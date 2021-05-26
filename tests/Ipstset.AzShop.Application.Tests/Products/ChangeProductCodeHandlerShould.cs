using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzProduct.Application.Tests.Fakes;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Products.ChangeProductCode;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;
using Xunit;

namespace Ipstset.AzShop.Application.Tests.Products
{
    public class ChangeProductCodeHandlerShould
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
            var sut = new ChangeProductCodeHandler(repos.ProductRepository, repos.ProductReadOnlyRepository);
            var newCode = $"PC_{Guid.NewGuid()}";
            var request = new ChangeProductCodeRequest
            {
                ProductId = product.Id.ToString(),
                ProductCode = newCode,
                User = new AppUser { Roles = new[] { "admin" } }
            };

            var actual = await sut.Handle(request, new System.Threading.CancellationToken());

            //assert
            Assert.IsType<ProductResponse>(actual);
            Assert.Equal(newCode, actual.ProductCode);
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
            var sut = new ChangeProductCodeHandler(repos.ProductRepository, repos.ProductReadOnlyRepository);
            var newCode = $"PC_{Guid.NewGuid()}";
            var request = new ChangeProductCodeRequest
            {
                ProductId = product.Id.ToString(),
                ProductCode = newCode,
                User = new AppUser()
            };

            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async Task Throw_NotFoundException_given_invalid_id()
        {
            var repos = new MockProductRepositories();
            var sut = new ChangeProductCodeHandler(repos.ProductRepository, repos.ProductReadOnlyRepository);
            var newCode = $"PC_{Guid.NewGuid()}";
            var request = new ChangeProductCodeRequest
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductCode = newCode,
                User = new AppUser { Roles = new[] { "admin" } }
            };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_BadRequestException_given_existing_productcode_in_shop()
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
            var sut = new ChangeProductCodeHandler(repos.ProductRepository, repos.ProductReadOnlyRepository);
            var request = new ChangeProductCodeRequest
            {
                ProductId = product.Id.ToString(),
                ProductCode = productCode,
                User = new AppUser()
            };

            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
