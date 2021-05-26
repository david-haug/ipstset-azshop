using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Application;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Products.GetProducts;
using Ipstset.AzShop.Domain.Products;

namespace Ipstset.AzProduct.Application.Tests.Fakes
{
    public class MockProductRepositories
    {
        public MockProductRepositories()
        {
            _products = new List<Product>();
            ProductRepository = new Repository(_products);
            ProductReadOnlyRepository = new ReadOnlyRepository(_products);
        }

        public IProductRepository ProductRepository { get; set; }
        public IProductReadOnlyRepository ProductReadOnlyRepository { get; set; }

        private List<Product> _products;

        private class Repository : IProductRepository
        {
            private List<Product> _products;
            public Repository(List<Product> products)
            {
                _products = products;
            }
            public Task DeleteAsync(Product product)
            {
                throw new NotImplementedException();
            }

            public Task<Product> GetAsync(Guid id)
            {
                var collection = _products.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(collection);
            }

            public Task SaveAsync(Product product)
            {
                _products.Add(product);
                return Task.CompletedTask;
            }
        }

        private class ReadOnlyRepository : IProductReadOnlyRepository
        {
            private List<Product> _products;

            public ReadOnlyRepository(List<Product> products)
            {
                _products = products;
            }

            public async Task<ProductResponse> GetByIdAsync(string id)
            {
                var product = _products.FirstOrDefault(o => o.Id.ToString() == id);
                if (product == null)
                    return null;

                var dto = new ProductResponse
                {
                    Id = product.Id.ToString(),
                    ShopId = product.ShopId.ToString(),
                    ProductCode = product.ProductCode,
                    Type = product.Type,
                    Copy = product.Copy,
                    IsActive = product.IsActive,
                    Pricing = product.Pricing,
                    Options = product.Options
                };

                return dto;
            }

            public async Task<QueryResult<ProductResponse>> GetProductsAsync(GetProductsRequest request)
            {
                var results = new List<ProductResponse>();
                foreach (var product in _products)
                {
                    var dto = new ProductResponse
                    {
                        Id = product.Id.ToString(),
                        ShopId = product.ShopId.ToString(),
                        ProductCode = product.ProductCode,
                        Type = product.Type,
                        Copy = product.Copy,
                        IsActive = product.IsActive,
                        Pricing = product.Pricing,
                        Options = product.Options
                    };

                    results.Add(dto);
                }

                return new QueryResult<ProductResponse> { Items = results, TotalRecords = results.Count, Limit = request.Limit, StartAfter = request.StartAfter };
            }

            public async Task<ProductResponse> GetByProductCodeAsync(string productCode, string shopId)
            {
                var product = _products.FirstOrDefault(o => o.ShopId.ToString() == shopId && o.ProductCode == productCode);
                if (product == null)
                    return null;

                var dto = new ProductResponse
                {
                    Id = product.Id.ToString(),
                    ShopId = product.ShopId.ToString(),
                    ProductCode = product.ProductCode,
                    Type = product.Type,
                    Copy = product.Copy,
                    IsActive = product.IsActive,
                    Pricing = product.Pricing,
                    Options = product.Options
                };

                return dto;
            }
        }
    }
}
