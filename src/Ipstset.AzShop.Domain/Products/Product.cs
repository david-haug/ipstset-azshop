using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Ipstset.AzShop.Domain.Products.Events;
using Ipstset.AzShop.Domain.ValueObjects;

namespace Ipstset.AzShop.Domain.Products
{
    public class Product:Entity
    {
        public Guid Id { get; private set; }

        public Guid ShopId { get; private set; }

        public string Type { get; private set; }

        public string ProductCode { get; private set; }

        public ProductCopy Copy { get; private set; }
        
        public bool IsActive { get; private set; }

        private readonly List<ProductOption> _options = new List<ProductOption>();
        public ReadOnlyCollection<ProductOption> Options => _options.AsReadOnly();

        private readonly List<ProductPrice> _pricing = new List<ProductPrice>();
        public ReadOnlyCollection<ProductPrice> Pricing => _pricing.AsReadOnly();

        public static Product Create(
            Guid shopId, 
            string type,
            string productCode, 
            ProductCopy copy,
            bool isActive, 
            IEnumerable<ProductOption> options, 
            IEnumerable<ProductPrice> pricing)
        {
            var product = Load(
                Guid.NewGuid(),
                shopId,
                type,
                productCode,
                copy,
                false, //if isActive == true when creating, then need to Activate
                options,
                pricing);

            //create event
            product.AddEvent(new ProductCreated(product));

            if (isActive)
            {
                product.Activate();
            }

            return product;
        }

        public static Product Load(
            Guid id,
            Guid shopId,
            string type,
            string productCode,
            ProductCopy copy,
            bool isActive,
            IEnumerable<ProductOption> options,
            IEnumerable<ProductPrice> pricing)
        {
            ValidateProductCode(productCode);

            var product = new Product
            {
                Id = id,
                ShopId = shopId,
                ProductCode = productCode,
                Copy = copy,
                IsActive = false //if isActive == true, validate after pricing set
            };

            SetType(product, type);

            if (pricing != null)
            {
                ValidatePricing(pricing);
                product._pricing.AddRange(pricing);
            }

            if (options != null)
            {
                ValidateOptions(options);
                product._options.AddRange(options);
            }

            if (isActive)
            {
                ValidateProductCanBeActive(product);
                product.IsActive = true;
            }

            return product;
        }

        public void UpdatePricing(IEnumerable<ProductPrice> pricing)
        {
            ValidatePricing(pricing);

            _pricing.Clear();
            _pricing.AddRange(pricing);
            
            AddEvent(new ProductPricingUpdated(this));
        }

        public void UpdateOptions(IEnumerable<ProductOption> options)
        {
            ValidateOptions(options);

            _options.Clear();
            _options.AddRange(options);

            AddEvent(new ProductOptionsUpdated(this));
        }

        public void Activate()
        {
            if (IsActive)
                throw new ApplicationException("Product is already activated");

            ValidateProductCanBeActive(this);
            
            IsActive = true;
            AddEvent(new ProductActivated(this));
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new ApplicationException("Product is already deactivated");

            IsActive = false;
            AddEvent(new ProductDeactivated(this));
        }

        public void UpdateCopy(ProductCopy info)
        {
            Copy = info;
            AddEvent(new ProductCopyUpdated(this));
        }

        public void ChangeProductCode(string productCode)
        {
            if(productCode == ProductCode)
                throw new ApplicationException("Product code is the same");

            ValidateProductCode(productCode);

            ProductCode = productCode;
            AddEvent(new ProductCodeChanged(this));
        }

        public void ChangeType(string type)
        {
            SetType(this, type);
            AddEvent(new ProductTypeChanged(this));
        }

        private static void SetType(Product product, string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("type");

            if (type == product.Type)
                throw new ApplicationException("Product type is the same");

            product.Type = type;
        }
        
        private static void ValidatePricing(IEnumerable<ProductPrice> pricing)
        {
            if(!pricing.Any())
                throw new ApplicationException("Pricing cannot be empty");

            var sorted = pricing.OrderBy(p => p.MinQuantity).ToList();
            var lastMinQty = 0;
            var lastMaxQty = 0;
            for (var i = 0; i < sorted.Count(); i++)
            {
                if (sorted[i].Amount < 0)
                    throw new ApplicationException($"pricing[{i}].Amount cannot be less than zero");
                if (sorted[i].MinQuantity <= 0)
                    throw new ApplicationException($"pricing[{i}].Price min quantity needs to be greater than zero");
                if (sorted[i].MinQuantity <= lastMinQty)
                    throw new ApplicationException($"pricing[{i}].MinQuantity cannot be same as other min quantity");
                if (sorted[i].MinQuantity > sorted[i].MaxQuantity)
                    throw new ApplicationException($"pricing[{i}].MinQuantity cannot be greater than max quantity");
                if (string.IsNullOrWhiteSpace(sorted[i].UomText))
                    throw new ArgumentException($"pricing[{i}].UomText");
                if (sorted[i].MinQuantity <= lastMaxQty)
                    throw new ApplicationException($"pricing[{i}].MinQuantity should be greater than last max quantity");

                lastMinQty = sorted[i].MinQuantity;
                lastMaxQty = sorted[i].MaxQuantity;
            }
        }

        private static void ValidateOptions(IEnumerable<ProductOption> options)
        {
            var optList = options.ToList();
            for (var i = 0; i < optList.Count(); i++)
            {
                if (string.IsNullOrWhiteSpace(optList[i].Name))
                    throw new ArgumentException($"options[{i}].Name");

                if(optList[i].Details != null && optList[i].Details.Any())
                    ValidateOptionDetails(optList[i].Details, i);
            }
        }

        private static void ValidateOptionDetails(IEnumerable<ProductOptionDetail> details, int optionIndex)
        {
            var detailList = details.ToList();
            for (var i = 0; i < detailList.Count(); i++)
            {
                if (string.IsNullOrWhiteSpace(detailList[i].Description))
                    throw new ArgumentException($"options[{optionIndex}].Details[{i}].Description");
                if (string.IsNullOrWhiteSpace(detailList[i].UomText))
                    throw new ArgumentException($"options[{optionIndex}].Details[{i}].UomText");
                if (detailList[i].AdditionalAmount < 0)
                    throw new ApplicationException($"options[{optionIndex}].Details[{i}].AdditionalAmount cannot be less than zero");
            }
        }

        private static void ValidateProductCode(string productCode)
        {
            //todo: should be done in Shop add product?
            //can repo be used?  repository.GetByProductCode...
            //void Shop.AddProduct(product,repository)???
            //let Application handle for now...

            if (string.IsNullOrWhiteSpace(productCode))
                throw new ArgumentException("productCode");
        }

        private static void ValidateProductCanBeActive(Product product)
        {
            //pricing required
            ValidatePricing(product.Pricing);
        }
        
    }
}
