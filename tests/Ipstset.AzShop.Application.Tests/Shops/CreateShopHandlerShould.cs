using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Shops;
using Ipstset.AzShop.Application.Shops.CreateShop;
using Ipstset.AzShop.Application.Tests.Fakes;
using Xunit;
namespace Ipstset.AzShop.Application.Tests.Shops
{
    public class CreateShopHandlerShould
    {
        [Fact]
        public void Return_ShopResponse_Given_Valid_Request()
        {
            var repos = new MockShopRepositories();
            var sut = new CreateShopHandler(repos.ShopRepository);

            var request = new CreateShopRequest
            {
                Name = "Test Category", 
                User = new AppUser { Roles = new[] { "admin" } }
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.IsType<ShopResponse>(actual);
        }

        [Fact]
        public async void Throw_NotAuthorized_Given_NonAdmin_User()
        {
            var repos = new MockShopRepositories();
            var sut = new CreateShopHandler(repos.ShopRepository);

            var request = new CreateShopRequest
            {
                Name = "Test Category",
                User = new AppUser( )

            };

            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
