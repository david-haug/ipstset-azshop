using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ipstset.AzShop.Application.Extensions;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;
using Xunit;

namespace Ipstset.AzShop.Application.Tests.Extensions
{
    public class ProductExtensionsShould
    {
        [Fact]
        public async void Return_product_response()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var info = new ProductCopy();
            var isActive = false;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 9.99, MinQuantity = 1, MaxQuantity = 4, UomText = "Each" },
                new ProductPrice { Amount = 7.99, MinQuantity = 5, MaxQuantity = 9, UomText = "Each" },
                new ProductPrice { Amount = 5.99, MinQuantity = 10, MaxQuantity = 14, UomText = "Each" },
                new ProductPrice { Amount = 2.99, MinQuantity = 15, MaxQuantity = 9999, UomText = "Each" }
            };
            var options = new List<ProductOption>
            {
                new ProductOption { Name = "Opt1" },
                new ProductOption
                {
                    Name = "Opt2",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail { Description = "det1", AdditionalAmount = (decimal) 4.99, UomText = "Per Each"},
                        new ProductOptionDetail { Description = "det2", AdditionalAmount = (decimal) 5.99, UomText = "Per Each"},
                        new ProductOptionDetail { Description = "det3", AdditionalAmount = (decimal) 3.99, UomText = "Per Each"}
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, info, isActive, options, pricing);
            var actual = sut.ToProductResponse();

            //event has correct data
            Assert.Equal(sut.Id.ToString(), actual.Id);
            Assert.Equal(sut.ShopId.ToString(), actual.ShopId);
            Assert.Equal(sut.Type, actual.Type);
            Assert.Equal(sut.ProductCode, actual.ProductCode);
            Assert.Equal(sut.IsActive, actual.IsActive);
            
            Assert.NotNull(actual.Copy);
            Assert.Equal(sut.Copy.Title, actual.Copy.Title);
            Assert.Equal(sut.Copy.Description, actual.Copy.Description);
            Assert.Equal(sut.Copy.ShortDescription, actual.Copy.ShortDescription);
            Assert.Equal(sut.Copy.ThumbnailUrl, actual.Copy.ThumbnailUrl);

            //pricing
            Assert.Equal(sut.Pricing.Count, actual.Pricing.Count());
            var i = 0;
            foreach (var price in actual.Pricing)
            {
                Assert.Equal(sut.Pricing[i].Amount, price.Amount);
                Assert.Equal(sut.Pricing[i].MinQuantity, price.MinQuantity);
                Assert.Equal(sut.Pricing[i].MaxQuantity, price.MaxQuantity);
                Assert.Equal(sut.Pricing[i].UomText, price.UomText);
                i++;
            }

            //options
            Assert.Equal(sut.Options.Count, actual.Options.Count());
            i = 0;
            var productOptions = sut.Options.ToList();
            foreach (var option in actual.Options)
            {
                Assert.Equal(sut.Options[i].Name, option.Name);

                if (option.Details != null)
                {
                    var ii = 0;
                    foreach (var detail in option.Details)
                    {
                        Assert.Equal(productOptions[i].Details.ToList()[ii].Description, detail.Description);
                        Assert.Equal(productOptions[i].Details.ToList()[ii].UomText, detail.UomText);
                        Assert.Equal(productOptions[i].Details.ToList()[ii].AdditionalAmount, detail.AdditionalAmount);
                        ii++;
                    }
                }

                i++;
            }
        }
    }
}
