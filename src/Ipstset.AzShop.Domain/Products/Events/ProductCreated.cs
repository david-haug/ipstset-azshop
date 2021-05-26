using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.ValueObjects;

namespace Ipstset.AzShop.Domain.Products.Events
{
    public class ProductCreated: IEvent
    {
        public ProductCreated(Product product)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            ProductId = product.Id;
            ShopId = product.ShopId;
            Type = product.Type;
            ProductCode = product.ProductCode;
            Copy = product.Copy;
            IsActive = product.IsActive;
            Pricing = product.Pricing;
            Options = product.Options;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
        public Guid ProductId { get; }
        public Guid ShopId { get; }
        public string Type { get; }
        public string ProductCode { get; }
        public ProductCopy Copy { get; }
        public bool IsActive { get; }
        public IEnumerable<ProductPrice> Pricing { get; }
        public IEnumerable<ProductOption> Options { get; }
    }
}
