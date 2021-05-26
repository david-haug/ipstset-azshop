using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.Shops;
using Ipstset.AzShop.Domain.ValueObjects;
using Ipstset.AzShop.Infrastructure.SqlDataAccess;
using Ipstset.AzShop.Infrastructure.Tests.Fakes;
using Xunit;

namespace Ipstset.AzShop.Infrastructure.Tests.SqlDataAccess
{
    public class ProductRepositoryShould
    {
        [Fact]
        public async void DB_add_new_product()
        {
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "UNIT_TEST_product_code";
            var info = new ProductCopy
            {
                Title = $"Unit test created shop {DateTime.Now}",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
            };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var sut = new ProductRepository(Settings.GetDbSettings(), new EventDispatcherStub());
            var product = Product.Load(id, shopId, type, productCode, info, isActive, options, pricing);
            await sut.SaveAsync(product);

            var saved = await sut.GetAsync(product.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async void DB_return_product_given_valid_id()
        {
            var shopId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            await SaveNewProduct(shopId,productId);

            var sut = new ProductRepository(Settings.GetDbSettings(), new EventDispatcherStub());
            var actual = await sut.GetAsync(productId);
            Assert.NotNull(actual);
            Assert.Equal(productId, actual.Id);
        }

        private async Task SaveNewProduct(Guid shopId, Guid productId)
        {
            var shopRepo = new ShopRepository(Settings.GetDbSettings(), new EventDispatcherStub());
            var shop = Shop.Load(shopId, $"Unit test created shop {DateTime.Now}");
            await shopRepo.SaveAsync(shop);

            var productRepo = new ProductRepository(Settings.GetDbSettings(), new EventDispatcherStub());

            var type = "tshirt";
            var productCode = "UNIT_TEST_product_code";
            var info = new ProductCopy
            {
                Title = $"Unit test created shop {DateTime.Now}",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
            };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };
            var product = Product.Load(productId, shopId, type, productCode, info, isActive, options, pricing);
            await productRepo.SaveAsync(product);

        }
    }
}
