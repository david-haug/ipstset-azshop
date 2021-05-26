using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.Products.Events;
using Ipstset.AzShop.Domain.Shops;
using Ipstset.AzShop.Domain.ValueObjects;
using Xunit;

namespace Ipstset.AzShop.Domain.Tests.Products
{
    public class ProductShould
    {
        [Fact]
        public void Create_given_valid_arguments()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var info = new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice> { new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" } };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" }};

            //act
            var sut = Product.Create(shopId, type, productCode, info, isActive, options, pricing);

            //Assert
            Assert.True(sut.Id != Guid.Empty);
            Assert.Equal(shopId, sut.ShopId);
            Assert.Equal(type, sut.Type);
            Assert.Equal(info.Title, sut.Copy.Title);
            Assert.Equal(info.Description, sut.Copy.Description);
            Assert.Equal(info.ShortDescription, sut.Copy.ShortDescription);
            Assert.Equal(info.ThumbnailUrl, sut.Copy.ThumbnailUrl);
            //pricing
            Assert.Equal(pricing.Count, sut.Pricing.Count);
            Assert.True(sut.Pricing[0].Amount == pricing[0].Amount);
            //options
            Assert.Equal(options.Count, sut.Options.Count);
            Assert.True(sut.Options[0].Name == options[0].Name);

        }

        [Fact]
        public void Load_given_valid_arguments()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var info = new ProductCopy
            {
                Title = "title",
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

            //act
            var sut = Product.Load(id, shopId, type, productCode, info, isActive, options, pricing);

            //Assert
            Assert.Equal(id, sut.Id);
            Assert.Equal(shopId, sut.ShopId);
            Assert.Equal(type, sut.Type);
            Assert.Equal(info.Title, sut.Copy.Title);
            Assert.Equal(info.Description, sut.Copy.Description);
            Assert.Equal(info.ShortDescription, sut.Copy.ShortDescription);
            Assert.Equal(info.ThumbnailUrl, sut.Copy.ThumbnailUrl);
            //pricing
            Assert.Equal(pricing.Count, sut.Pricing.Count);
            Assert.True(sut.Pricing[0].Amount == pricing[0].Amount);
            //options
            Assert.Equal(options.Count, sut.Options.Count);
            Assert.True(sut.Options[0].Name == options[0].Name);
        }

        [Fact]
        public void Add_ProductCreated_event()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var info = new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice> { new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" } };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var sut = Product.Create(shopId, type, productCode, info, isActive, options, pricing);
            var events = sut.DequeueEvents();
            var @event = (ProductCreated)events.FirstOrDefault(e => e is ProductCreated);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.Id != Guid.Empty);
            Assert.Equal(shopId, @event.ShopId);
            Assert.Equal(type, @event.Type);
            Assert.Equal(info.Title, @event.Copy.Title);
            Assert.Equal(info.Description, @event.Copy.Description);
            Assert.Equal(info.ShortDescription, @event.Copy.ShortDescription);
            Assert.Equal(info.ThumbnailUrl, @event.Copy.ThumbnailUrl);
            //pricing
            Assert.Equal(pricing.Count, @event.Pricing.Count());
            var eventPricing = @event.Pricing.ToList();
            Assert.True(eventPricing[0].Amount == pricing[0].Amount);
            //options
            Assert.Equal(options.Count, @event.Options.Count());
            var eventOptions = @event.Options.ToList();
            Assert.True(eventOptions[0].Name == options[0].Name);
        }

        [Fact]
        public void Throw_ArgumentException_given_no_ProductCode()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "";
            var info = new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice> { new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each"} };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var ex = Assert.Throws<ArgumentException>(() => Product.Create(shopId, type, productCode, info, isActive, options, pricing));
            Assert.Equal("productCode", ex.Message);
        }

        #region Pricing
        [Fact]
        public void Update_Pricing()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var sut = Product.Load(id, shopId, type, productCode, null, true, null, pricing);

            //act
            var updatedPricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 10.00, MinQuantity = 1, MaxQuantity = 5, UomText = "Each" },
                new ProductPrice { Amount = 7.50, MinQuantity = 6, MaxQuantity = 10, UomText = "Each" },
                new ProductPrice { Amount = 5.00, MinQuantity = 11, MaxQuantity = 99999, UomText = "Each" },
            };

            sut.UpdatePricing(updatedPricing);

            //Assert
            Assert.Equal(updatedPricing.Count, sut.Pricing.Count);
            var i = 0;
            foreach (var price in updatedPricing)
            {
                Assert.Equal(price.Amount, sut.Pricing[i].Amount);
                Assert.Equal(price.MinQuantity, sut.Pricing[i].MinQuantity);
                Assert.Equal(price.MaxQuantity, sut.Pricing[i].MaxQuantity);
                Assert.Equal(price.UomText, sut.Pricing[i].UomText);
                i++;
            }
        }

        [Fact]
        public void Add_ProductPricingUpdated_event()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var sut = Product.Load(id, shopId, type, productCode, null, true, null, pricing);

            //act
            var updatedPricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 10.00, MinQuantity = 1, MaxQuantity = 5, UomText = "Each" },
                new ProductPrice { Amount = 7.50, MinQuantity = 6, MaxQuantity = 10, UomText = "Each" },
                new ProductPrice { Amount = 5.00, MinQuantity = 11, MaxQuantity = 99999, UomText = "Each" },
            };

            sut.UpdatePricing(updatedPricing);
            var events = sut.DequeueEvents();
            var @event = (ProductPricingUpdated)events.FirstOrDefault(e => e is ProductPricingUpdated);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.Id != Guid.Empty);
            Assert.Equal(id, @event.ProductId);
            //pricing
            Assert.Equal(updatedPricing.Count, @event.Pricing.Count());
            var eventPricing = @event.Pricing.ToList();
            Assert.Equal(updatedPricing.Count, sut.Pricing.Count);
            var i = 0;
            foreach (var price in updatedPricing)
            {
                Assert.Equal(price.Amount, eventPricing[i].Amount);
                Assert.Equal(price.MinQuantity, eventPricing[i].MinQuantity);
                Assert.Equal(price.MaxQuantity, eventPricing[i].MaxQuantity);
                Assert.Equal(price.UomText, eventPricing[i].UomText);
                i++;
            }
        }

        [Fact]
        public void Throw_ApplicationException_given_pricing_with_amount_less_than_zero()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "TEST";
            var info = new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice> { new ProductPrice { Amount = -1, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" } };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var ex = Assert.Throws<ApplicationException>(() => Product.Create(shopId, type, productCode, info, isActive, options, pricing));
            Assert.Equal("pricing[0].Amount cannot be less than zero", ex.Message);
        }

        [Fact]
        public void Throw_ApplicationException_given_pricing_with_min_quantity_less_than_zero()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "TEST";
            var info = new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice> { new ProductPrice { Amount = 1, MinQuantity = -1, MaxQuantity = 9999, UomText = "Each" } };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var ex = Assert.Throws<ApplicationException>(() => Product.Create(shopId, type, productCode, info, isActive, options, pricing));
            Assert.Equal("pricing[0].Price min quantity needs to be greater than zero", ex.Message);
        }

        [Fact]
        public void Throw_ApplicationException_given_pricing_with_same_min_quantities()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "TEST";
            var info = new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 2, MinQuantity = 1, MaxQuantity = 10, UomText = "Each" },
                new ProductPrice { Amount = 1, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
            };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var ex = Assert.Throws<ApplicationException>(() => Product.Create(shopId, type, productCode, info, isActive, options, pricing));
            Assert.Equal("pricing[1].MinQuantity cannot be same as other min quantity", ex.Message);
        }

        [Fact]
        public void Throw_ApplicationException_given_price_with_min_quantity_greater_than_max_quantity()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "TEST";
            var info = new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 2, MinQuantity = 10, MaxQuantity = 5, UomText = "Each" },
            };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var ex = Assert.Throws<ApplicationException>(() => Product.Create(shopId, type, productCode, info, isActive, options, pricing));
            Assert.Equal("pricing[0].MinQuantity cannot be greater than max quantity", ex.Message);
        }

        [Fact]
        public void Throw_ApplicationException_given_price_with_min_quantity_less_than_previous_max_quantity()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "TEST";
            var info = new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short_desc",
                ThumbnailUrl = "thumb"
            };
            var isActive = true;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 2, MinQuantity = 1, MaxQuantity = 4, UomText = "Each" },
                new ProductPrice { Amount = 2, MinQuantity = 5, MaxQuantity = 10, UomText = "Each" },
                new ProductPrice { Amount = 1, MinQuantity = 6, MaxQuantity = 9999, UomText = "Each" }
            };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var ex = Assert.Throws<ApplicationException>(() => Product.Create(shopId, type, productCode, info, isActive, options, pricing));
            Assert.Equal("pricing[2].MinQuantity should be greater than last max quantity", ex.Message);
        }
        #endregion Pricing

        #region Options

        [Fact]
        public void Update_Options()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, true, options, pricing);

            //act
            var updatedOptions = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt2",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                },
                new ProductOption
                {
                    Name = "Opt3",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M }
                    }
                }
            };

            sut.UpdateOptions(updatedOptions);

            //Assert
            Assert.Equal(updatedOptions.Count, sut.Options.Count);
            var i = 0;
            foreach (var option in updatedOptions)
            {
                Assert.Equal(option.Name, sut.Options[i].Name);
                Assert.Equal(option.Details.Count(), sut.Options[i].Details.Count());
                i++;
            }
        }

        [Fact]
        public void Add_ProductOptionsUpdated_event()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, true, options, pricing);

            //act
            var updatedOptions = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt2",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                },
                new ProductOption
                {
                    Name = "Opt3",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M }
                    }
                }
            };

            sut.UpdateOptions(updatedOptions);
            var events = sut.DequeueEvents();
            var @event = (ProductOptionsUpdated)events.FirstOrDefault(e => e is ProductOptionsUpdated);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.Id != Guid.Empty);
            Assert.Equal(id, @event.ProductId);
            //pricing
            Assert.Equal(updatedOptions.Count, @event.Options.Count());
            var eventOptions = @event.Options.ToList();
            Assert.Equal(updatedOptions.Count, sut.Options.Count);
            var i = 0;
            foreach (var option in updatedOptions)
            {
                Assert.Equal(option.Name, sut.Options[i].Name);
                Assert.Equal(option.Details.Count(), sut.Options[i].Details.Count());
                i++;
            }
        }

        [Fact]
        public void Throw_ArgumentException_given_option_with_no_name()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 1.01M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M }
                    }
                },
                new ProductOption
                {
                    Name = "",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 1.01M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M }
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(() => Product.Create(shopId, type, productCode, null, true, options, pricing));
            Assert.Equal("options[1].Name", ex.Message);
        }

        [Fact]
        public void Throw_ApplicationException_given_option_with_additional_amount_less_than_zero()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 1.01M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M }
                    }
                },
                new ProductOption
                {
                    Name = "Opt2",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 1.01M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = -3.00M }
                    }
                }
            };

            var ex = Assert.Throws<ApplicationException>(() => Product.Create(shopId, type, productCode, null, true, options, pricing));
            Assert.Contains("AdditionalAmount cannot be less than zero", ex.Message);
        }

        [Fact]
        public void Throw_ArgumentException_given_option_with_no_description()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 1.01M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M }
                    }
                },
                new ProductOption
                {
                    Name = "Opt2",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {UomText = "Each", AdditionalAmount = 1.01M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M }
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(() => Product.Create(shopId, type, productCode, null, true, options, pricing));
            Assert.Equal("options[1].Details[0].Description", ex.Message);
        }

        [Fact]
        public void Throw_ArgumentException_given_option_with_no_uom_text()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 1.01M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M }
                    }
                },
                new ProductOption
                {
                    Name = "Opt2",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1",UomText = "Each", AdditionalAmount = 1.01M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M },
                        new ProductOptionDetail {Description = "desc3", AdditionalAmount = 3.00M }
                    }
                }
            };

            var ex = Assert.Throws<ArgumentException>(() => Product.Create(shopId, type, productCode, null, true, options, pricing));
            Assert.Equal("options[1].Details[2].UomText", ex.Message);
        }

        #endregion Options

        #region Activate/Deactivate
        [Fact]
        public void Set_IsActive_to_true_when_activated()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);

            //act
            sut.Activate();
            Assert.True(sut.IsActive);
        }

        [Fact]
        public void Add_ProductActivated_event()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);

            //act
            sut.Activate();
            var events = sut.DequeueEvents();
            var @event = (ProductActivated)events.FirstOrDefault(e => e is ProductActivated);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.Id != Guid.Empty);
            Assert.Equal(id, @event.ProductId);
        }

        [Fact]
        public void Throw_ApplicationException_when_activating_active_product()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 1.01M }
                    }
                }
            };

            var sut = Product.Create(shopId, type, productCode, null, true, options, pricing);

            var ex = Assert.Throws<ApplicationException>(() => sut.Activate());
            Assert.Equal("Product is already activated", ex.Message);
        }

        [Fact]
        public void Add_ProductActivated_event_when_creating_active_product()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                }
            };

            //act
            var sut = Product.Create(shopId, type, productCode, null, true, options, pricing);
            var events = sut.DequeueEvents();
            var @event = (ProductActivated)events.FirstOrDefault(e => e is ProductActivated);
            Assert.NotNull(@event);
        }

        [Fact]
        public void Set_IsActive_to_false_when_deactivated()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, true, options, pricing);

            //act
            sut.Deactivate();
            Assert.False(sut.IsActive);
        }

        [Fact]
        public void Add_ProductDectivated_event()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, true, options, pricing);

            //act
            sut.Deactivate();
            var events = sut.DequeueEvents();
            var @event = (ProductDeactivated)events.FirstOrDefault(e => e is ProductDeactivated);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.Id != Guid.Empty);
            Assert.Equal(id, @event.ProductId);
        }

        [Fact]
        public void Throw_ApplicationException_when_deactivating_deactivated_product()
        {
            //arrange
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 1.01M }
                    }
                }
            };

            var sut = Product.Create(shopId, type, productCode, null, false, options, pricing);

            var ex = Assert.Throws<ApplicationException>(() => sut.Deactivate());
            Assert.Equal("Product is already deactivated", ex.Message);
        }
        #endregion Activate/Deactivate

        [Fact]
        public void Update_product_copy()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);
            sut.UpdateCopy(new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short desc",
                ThumbnailUrl = "thumb"
            });

            //act
            Assert.NotNull(sut.Copy);
            Assert.Equal("title",sut.Copy.Title);
        }

        [Fact]
        public void Add_ProductCopyUpdated_event()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M },
                        new ProductOptionDetail {Description = "desc2", UomText = "Each", AdditionalAmount = 3.00M}
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);

            //act
            sut.UpdateCopy(new ProductCopy
            {
                Title = "title",
                Description = "desc",
                ShortDescription = "short desc",
                ThumbnailUrl = "thumb"
            });
            var events = sut.DequeueEvents();
            var @event = (ProductCopyUpdated)events.FirstOrDefault(e => e is ProductCopyUpdated);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.Id != Guid.Empty);
            Assert.Equal(id, @event.ProductId);
            Assert.Equal(sut.Copy.Title, @event.Copy.Title);
            Assert.Equal(sut.Copy.ThumbnailUrl, @event.Copy.ThumbnailUrl);
            Assert.Equal(sut.Copy.Description, @event.Copy.Description); 
            Assert.Equal(sut.Copy.ShortDescription, @event.Copy.ShortDescription);
        }

        [Fact]
        public void Change_product_code()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M }
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);
            var newProductCode = "new";
            sut.ChangeProductCode(newProductCode);

            //act
            Assert.Equal(newProductCode,sut.ProductCode);
        }

        [Fact]
        public void Add_ProductCodeChanged_event()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M }
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);
            var newProductCode = "new";


            //act
            sut.ChangeProductCode(newProductCode);
            var events = sut.DequeueEvents();
            var @event = (ProductCodeChanged)events.FirstOrDefault(e => e is ProductCodeChanged);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.Id != Guid.Empty);
            Assert.Equal(id, @event.ProductId);
            Assert.Equal(sut.ProductCode, @event.ProductCode);
        }

        [Fact]
        public void Throw_ArgumentException_when_changing_to_blank_product_code()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M }
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);
            var ex = Assert.Throws<ArgumentException>(() => sut.ChangeProductCode(" "));
            Assert.Equal("productCode", ex.Message);
        }

        [Fact]
        public void Throw_ApplicationException_when_changing_to_same_product_code()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M }
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);
            var ex = Assert.Throws<ApplicationException>(() => sut.ChangeProductCode("product_code"));
            Assert.Equal("Product code is the same", ex.Message);
        }

        [Fact]
        public void Change_product_type()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "none";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M }
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);
            var newType = "tshirt";
            sut.ChangeType(newType);

            //act
            Assert.Equal(newType, sut.Type);
        }

        [Fact]
        public void Add_ProductTypeChanged_event()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "none";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M }
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);
            var newType = "tshirt";
            sut.ChangeType(newType);

            var events = sut.DequeueEvents();
            var @event = (ProductTypeChanged)events.FirstOrDefault(e => e is ProductTypeChanged);
            Assert.NotNull(@event);

            //event has correct data
            Assert.True(@event.Id != Guid.Empty);
            Assert.Equal(id, @event.ProductId);
            Assert.Equal(sut.Type, @event.Type);
        }

        [Fact]
        public void Throw_ApplicationException_when_changing_to_same_product_type()
        {
            //arrange
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "none";
            var productCode = "product_code";
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 99999, UomText = "Each"}
            };

            var options = new List<ProductOption>
            {
                new ProductOption
                {
                    Name = "Opt1",
                    Details = new List<ProductOptionDetail>
                    {
                        new ProductOptionDetail {Description = "desc1", UomText = "Each", AdditionalAmount = 2.00M }
                    }
                }
            };

            var sut = Product.Load(id, shopId, type, productCode, null, false, options, pricing);

            var ex = Assert.Throws<ApplicationException>(() => sut.ChangeType(type));
            Assert.Equal("Product type is the same", ex.Message);
        }
    }
}
