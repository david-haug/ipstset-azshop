using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Application;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Products.GetProducts;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.Shops;
using Ipstset.AzShop.Domain.ValueObjects;
using Ipstset.AzShop.Infrastructure.SqlDataAccess;
using Ipstset.AzShop.Infrastructure.Tests.Fakes;
using Xunit;

namespace Ipstset.AzShop.Infrastructure.Tests.SqlDataAccess
{
    public class ProductReadOnlyRepositoryShould
    {
        [Fact]
        public async void DB_return_ProductResponse_given_valid_id()
        {
            var shopId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            await SaveNewProduct(shopId, productId);

            var sut = new ProductReadOnlyRepository(Settings.GetDbSettings());
            var product = await sut.GetByIdAsync(productId.ToString());
            Assert.NotNull(product);
            Assert.Equal(productId.ToString(), product.Id);
            Assert.IsType<ProductResponse>(product);
        }

        [Fact]
        public async void DB_return_products_given_valid_request()
        {
            var request = new GetProductsRequest { User = new AppUser() };
            var sut = new ProductReadOnlyRepository(Settings.GetDbSettings());
            var actual = await sut.GetProductsAsync(request);
            Assert.NotNull(actual);
            Assert.IsType<QueryResult<ProductResponse>>(actual);
        }

        

        [Fact]
        public async void DB_return_ProductResponse_given_valid_product_code_and_shop_id()
        {
            var shopId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var productCode = $"PC_{Guid.NewGuid()}";
            await SaveNewProduct(shopId, productId, productCode);

            var sut = new ProductReadOnlyRepository(Settings.GetDbSettings());
            var product = await sut.GetByProductCodeAsync(productCode, shopId.ToString());
            Assert.NotNull(product);
            Assert.Equal(productId.ToString(), product.Id);
            Assert.IsType<ProductResponse>(product);
        }

        [Fact]
        public async void DB_return_null_given_invalid_product_code_and_shop_id()
        {
            var shopId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var productCode = $"PC_{Guid.NewGuid()}";
            await SaveNewProduct(shopId, productId, productCode);

            var sut = new ProductReadOnlyRepository(Settings.GetDbSettings());
            var product = await sut.GetByProductCodeAsync(Guid.NewGuid().ToString(), shopId.ToString());
            Assert.Null(product);
        }


        private async Task SaveNewProduct(Guid shopId, Guid productId, string productCode = "UNIT_TEST_product_code")
        {
            var shopRepo = new ShopRepository(Settings.GetDbSettings(), new EventDispatcherStub());
            var shop = Shop.Load(shopId, $"Unit test created shop {DateTime.Now}");
            await shopRepo.SaveAsync(shop);

            var productRepo = new ProductRepository(Settings.GetDbSettings(), new EventDispatcherStub());

            var type = "tshirt";
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
