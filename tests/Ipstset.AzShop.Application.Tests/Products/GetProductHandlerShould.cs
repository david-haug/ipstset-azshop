using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzProduct.Application.Tests.Fakes;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Products.GetProduct;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;
using Xunit;

namespace Ipstset.AzShop.Application.Tests.Products
{
    public class GetProductHandlerShould
    {
        [Fact]
        public async void Return_PropductResponse_given_valid_id()
        {
            var repos = new MockProductRepositories();
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

            var product = Product.Load(id, shopId, type, productCode, info, isActive, options, pricing);
            await repos.ProductRepository.SaveAsync(product);

            var sut = new GetProductHandler(repos.ProductReadOnlyRepository);
            var request = new GetProductRequest { Id = product.Id.ToString(), User = new AppUser { Roles = new[] { "admin" } } };
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.Equal(request.Id, actual.Id);
        }

        [Fact]
        public async Task Throw_NotFoundException_Given_Invalid_Id()
        {
            var repos = new MockProductRepositories();
            var sut = new GetProductHandler(repos.ProductReadOnlyRepository);
            var request = new GetProductRequest { Id = Guid.NewGuid().ToString(), User = new AppUser { Roles = new[] { "admin" } } };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        //[Fact]
        //public async Task Throw_NotAuthorizedException_when_user_has_no_shop_access()
        //{
        //    var repos = new MockProductRepositories();
        //    var id = Guid.NewGuid();
        //    var shopId = Guid.NewGuid();
        //    var type = "tshirt";
        //    var productCode = "product_code";
        //    var info = new ProductCopy
        //    {
        //        Title = "title",
        //        Description = "desc",
        //        ShortDescription = "short_desc",
        //        ThumbnailUrl = "thumb"
        //    };
        //    var isActive = true;
        //    var pricing = new List<ProductPrice>
        //    {
        //        new ProductPrice { Amount = 0.99, MinQuantity = 1, MaxQuantity = 9999, UomText = "Each" }
        //    };
        //    var options = new List<ProductOption> { new ProductOption { Name = "Opt1" } };

        //    var product = Product.Load(id, shopId, type, productCode, info, isActive, options, pricing);
        //    await repos.ProductRepository.SaveAsync(product);

        //    var sut = new GetProductHandler(repos.ProductReadOnlyRepository);
        //    var request = new GetProductRequest { Id = id.ToString(), User = new AppUser() };
        //    await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        //}
    }
}
