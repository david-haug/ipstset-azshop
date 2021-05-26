using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzProduct.Application.Tests.Fakes;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Products.UpdateOptions;
using Ipstset.AzShop.Application.Products.UpdatePricing;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;
using Xunit;

namespace Ipstset.AzShop.Application.Tests.Products
{
    public class UpdateOptionsHandlerShould
    {
        [Fact]
        public async void Return_ProductResponse_given_valid_request()
        {
            //arrange
            var repos = new MockProductRepositories();
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var info = new ProductCopy();
            var isActive = true;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
            };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var product = Product.Load(id, shopId, type, productCode, info, isActive, options, pricing);
            await repos.ProductRepository.SaveAsync(product);

            //act
            var sut = new UpdateOptionsHandler(repos.ProductRepository);
            var request = new UpdateOptionsRequest
            {
                ProductId = product.Id.ToString(),
                User = new AppUser { Roles = new[] { "admin" } },
                Options = new List<ProductOption>
                {
                    new ProductOption { 
                        Name = "Opt1", 
                        Details = new List<ProductOptionDetail>
                        {
                            new ProductOptionDetail { Description = "det1", AdditionalAmount = 1.99M, UomText = "each" },
                            new ProductOptionDetail { Description = "det2", AdditionalAmount = 1.99M, UomText = "each" }
                        }
                    },
                    new ProductOption {
                        Name = "Opt2",
                        Details = new List<ProductOptionDetail>
                        {
                            new ProductOptionDetail { Description = "det1", AdditionalAmount = 1.99M, UomText = "each" }
                        }
                    },
                    new ProductOption {
                        Name = "Opt3",
                        Details = new List<ProductOptionDetail>
                        {
                            new ProductOptionDetail { Description = "det1", AdditionalAmount = 1.99M, UomText = "each" },
                            new ProductOptionDetail { Description = "det2", AdditionalAmount = 1.99M, UomText = "each" },
                            new ProductOptionDetail { Description = "det3", AdditionalAmount = 1.99M, UomText = "each" },
                            new ProductOptionDetail { Description = "det4", AdditionalAmount = 1.99M, UomText = "each" }
                        }
                    },
                }
            };

            var actual = await sut.Handle(request, new System.Threading.CancellationToken());

            //assert
            Assert.IsType<ProductResponse>(actual);
            Assert.Equal(request.Options.Count(), actual.Options.Count());
            var actualOptions = actual.Options.ToList();
            var i = 0;
            foreach (var option in request.Options.ToList())
            {
                Assert.Equal(option.Name, actualOptions[i].Name);
                Assert.Equal(option.Details.Count(), actualOptions[i].Details.Count());

                var ii = 0;
                foreach (var detail in request.Options.ToList()[i].Details)
                {
                    Assert.Equal(detail.Description, actualOptions[i].Details.ToList()[ii].Description);
                    Assert.Equal(detail.AdditionalAmount, actualOptions[i].Details.ToList()[ii].AdditionalAmount);
                    Assert.Equal(detail.UomText, actualOptions[i].Details.ToList()[ii].UomText);
                    ii++;
                }

                i++;
            }
        }

        [Fact]
        public async void Throw_NotAuthorizedException_given_non_admin_user()
        {
            //arrange
            var repos = new MockProductRepositories();
            var id = Guid.NewGuid();
            var shopId = Guid.NewGuid();
            var type = "tshirt";
            var productCode = "product_code";
            var info = new ProductCopy();
            var isActive = true;
            var pricing = new List<ProductPrice>
            {
                new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
            };
            var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

            var product = Product.Load(id, shopId, type, productCode, info, isActive, options, pricing);
            await repos.ProductRepository.SaveAsync(product);

            //act
            var sut = new UpdateOptionsHandler(repos.ProductRepository);
            var request = new UpdateOptionsRequest
            {
                ProductId = product.Id.ToString(),
                User = new AppUser(),
                Options = new List<ProductOption>
                {
                    new ProductOption {
                        Name = "Opt1",
                        Details = new List<ProductOptionDetail>
                        {
                            new ProductOptionDetail { Description = "det1", AdditionalAmount = 1.99M, UomText = "each" },
                            new ProductOptionDetail { Description = "det2", AdditionalAmount = 1.99M, UomText = "each" }
                        }
                    },
                    new ProductOption {
                        Name = "Opt2",
                        Details = new List<ProductOptionDetail>
                        {
                            new ProductOptionDetail { Description = "det1", AdditionalAmount = 1.99M, UomText = "each" }
                        }
                    }
                }
            };

            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async Task Throw_NotFoundException_given_invalid_id()
        {
            var repos = new MockProductRepositories();
            var sut = new UpdateOptionsHandler(repos.ProductRepository);
            var request = new UpdateOptionsRequest
            {
                ProductId = Guid.NewGuid().ToString(),
                User = new AppUser { Roles = new[] { "admin" } },
                Options = new List<ProductOption>
                {
                    new ProductOption {
                        Name = "Opt1",
                        Details = new List<ProductOptionDetail>
                        {
                            new ProductOptionDetail { Description = "det1", AdditionalAmount = 1.99M, UomText = "each" },
                            new ProductOptionDetail { Description = "det2", AdditionalAmount = 1.99M, UomText = "each" }
                        }
                    }
                }
            };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
