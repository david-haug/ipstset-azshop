using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzProduct.Application.Tests.Fakes;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Products.GetProducts;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;
using Xunit;

namespace Ipstset.AzShop.Application.Tests.Products
{
    public class GetProductsHandlerShould
    {
        [Fact]
        public async void Return_QueryResult_given_valid_nonadmin_request()
        {
            var repos = new MockProductRepositories();
            var products = GetExistingProducts();
            foreach (var product in products)
                await repos.ProductRepository.SaveAsync(product);

            var shopId = Guid.NewGuid().ToString();
            var request = new GetProductsRequest
            {
                ShopId = shopId,
                User = new AppUser { Roles = new [] { $"readonly_{shopId}"}}
            };

            var sut = new GetProductsHandler(repos.ProductReadOnlyRepository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<QueryResult<ProductResponse>>(actual);
            Assert.Equal(products.ToList().Count, actual.Items.Count());
        }

        [Fact]
        public async void Return_QueryResult_given_admin_request_without_shop_id()
        {
            var repos = new MockProductRepositories();
            var products = GetExistingProducts();
            foreach (var product in products)
                await repos.ProductRepository.SaveAsync(product);

            var request = new GetProductsRequest
            {
                User = new AppUser { Roles = new[] { "admin" } }
            };

            var sut = new GetProductsHandler(repos.ProductReadOnlyRepository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<QueryResult<ProductResponse>>(actual);
            Assert.Equal(products.ToList().Count, actual.Items.Count());
        }

        //[Fact]
        //public async Task Throw_BadRequestException_given_request_by_nonadmin_with_no_shop_id()
        //{
        //    var repos = new MockProductRepositories();
        //    var request = new GetProductsRequest
        //    {
        //        User = new AppUser()
        //    };

        //    var sut = new GetProductsHandler(repos.ProductReadOnlyRepository);
        //    await Assert.ThrowsAsync<BadRequestException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        //}

        //[Fact]
        //public async Task Throw_NotAuthorizedException_given_request_with_no_shop_access()
        //{
        //    var repos = new MockProductRepositories();
        //    var request = new GetProductsRequest
        //    {
        //        ShopId = Guid.NewGuid().ToString(),
        //        User = new AppUser()
        //    };

        //    var sut = new GetProductsHandler(repos.ProductReadOnlyRepository);
        //    await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        //}

        private IEnumerable<Product> GetExistingProducts()
        {
            var products = new List<Product>();
            var shopId = Guid.NewGuid();

            products.Add(Product.Load(
                Guid.NewGuid(), 
                shopId, 
                "tshirt",
                "PRODUCT1",
                new ProductCopy(),
                false,
                null,
                new List<ProductPrice>
                {
                    new ProductPrice { Amount = 9.99, MinQuantity = 1, MaxQuantity = 5, UomText = "Each" },
                    new ProductPrice { Amount = 5.99, MinQuantity = 6, MaxQuantity = 9999, UomText = "Each" },
                }
                ));
            products.Add(Product.Load(
                Guid.NewGuid(),
                shopId,
                "tshirt",
                "PRODUCT2",
                new ProductCopy(),
                false,
                null,
                new List<ProductPrice>
                {
                    new ProductPrice { Amount = 9.99, MinQuantity = 1, MaxQuantity = 5, UomText = "Each" },
                    new ProductPrice { Amount = 5.99, MinQuantity = 6, MaxQuantity = 9999, UomText = "Each" },
                }
            ));
            products.Add(Product.Load(
                Guid.NewGuid(),
                shopId,
                "tshirt",
                "PRODUCT3",
                new ProductCopy(),
                false,
                null,
                new List<ProductPrice>
                {
                    new ProductPrice { Amount = 9.99, MinQuantity = 1, MaxQuantity = 5, UomText = "Each" },
                    new ProductPrice { Amount = 5.99, MinQuantity = 6, MaxQuantity = 9999, UomText = "Each" },
                }
            ));

            return products;
        }
    }
}
