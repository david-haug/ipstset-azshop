using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzProduct.Application.Tests.Fakes;
using Ipstset.AzShop.Application.Specifications;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;
using Xunit;

namespace Ipstset.AzShop.Application.Tests.Specifications
{
    public class ProductCodeIsUniqueToShopShould
    {
        [Fact]
        public async void Return_false_given_existing_productcode_and_shopid()
        {
            //arrange
            var repos = new MockProductRepositories();
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var info = new ProductCopy();
            var isActive = false;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
            };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var product = Product.Load(id, shopId, type, productCode, info, isActive, options, pricing);
            await repos.ProductRepository.SaveAsync(product);


            var sut = new ProductCodeIsUniqueToShop(repos.ProductReadOnlyRepository);
            var actual = sut.IsSatisifiedBy(shopId, productCode);
            Assert.False(actual);
        }

        [Fact]
        public async void Return_true_given_new_productcode_and_shopid()
        {
            //arrange
            var repos = new MockProductRepositories();
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var info = new ProductCopy();
            var isActive = false;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
            };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var product = Product.Load(id, shopId, type, productCode, info, isActive, options, pricing);
            await repos.ProductRepository.SaveAsync(product);


            var sut = new ProductCodeIsUniqueToShop(repos.ProductReadOnlyRepository);
            var actual = sut.IsSatisifiedBy(shopId, "new");
            Assert.True(actual);
        }
    }
}
