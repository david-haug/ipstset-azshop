using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzProduct.Application.Tests.Fakes;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Products.CreateProduct;
using Ipstset.AzShop.Application.Tests.Fakes;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.Shops;
using Ipstset.AzShop.Domain.ValueObjects;
using Xunit;

namespace Ipstset.AzShop.Application.Tests.Products
{
    public class CreateProductHandlerShould
    {
        [Fact]
        public async void Return_ProductResponse_given_valid_request()
        {
            //create valid shop
            var shopRepos = new MockShopRepositories();
            var shop = Shop.Load(Guid.NewGuid(), "name");
            await shopRepos.ShopRepository.SaveAsync(shop);

            var productRepos = new MockProductRepositories();

            var sut = new CreateProductHandler(productRepos.ProductRepository, shopRepos.ShopReadOnlyRepository);

            var request = new CreateProductRequest
            {
                ShopId = shop.Id.ToString(),
                ProductCode = "product_code",
                Type= "none",
                Copy = new ProductCopy(),
                IsActive = false,
                Pricing = new List<ProductPrice>
                {
                    new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
                },
                Options = new List<ProductOption> { new ProductOption { Name = "Opt1" } },
                User = new AppUser { Roles = new[] { "admin" } }
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.IsType<ProductResponse>(actual);
        }

        [Fact]
        public async void Throw_NotAuthorized_Given_NonAdmin_User()
        {
            //create valid shop
            var shopRepos = new MockShopRepositories();
            var shop = Shop.Load(Guid.NewGuid(), "name");
            await shopRepos.ShopRepository.SaveAsync(shop);

            var productRepos = new MockProductRepositories();

            var sut = new CreateProductHandler(productRepos.ProductRepository, shopRepos.ShopReadOnlyRepository);

            var request = new CreateProductRequest
            {
                ShopId = shop.Id.ToString(),
                ProductCode = "product_code",
                Type = "none",
                Copy = new ProductCopy(),
                IsActive = false,
                Pricing = new List<ProductPrice>
                {
                    new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
                },
                Options = new List<ProductOption> { new ProductOption { Name = "Opt1" } },
                User = new AppUser()
            };

            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async Task Throw_BadRequestException_given_invalid_id()
        {
            //create valid shop
            var shopRepos = new MockShopRepositories();
            var productRepos = new MockProductRepositories();

            var sut = new CreateProductHandler(productRepos.ProductRepository, shopRepos.ShopReadOnlyRepository);

            var request = new CreateProductRequest
            {
                ShopId = Guid.NewGuid().ToString(),
                ProductCode = "product_code",
                Type = "none",
                Copy = new ProductCopy(),
                IsActive = false,
                Pricing = new List<ProductPrice>
                {
                    new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
                },
                Options = new List<ProductOption> { new ProductOption { Name = "Opt1" } },
                User = new AppUser { Roles = new[] { "admin" } }
            };

            await Assert.ThrowsAsync<BadRequestException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
