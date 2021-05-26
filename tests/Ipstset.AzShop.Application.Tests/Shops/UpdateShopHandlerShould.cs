using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Shops;
using Ipstset.AzShop.Application.Shops.GetShop;
using Ipstset.AzShop.Application.Shops.UpdateShop;
using Ipstset.AzShop.Application.Tests.Fakes;
using Ipstset.AzShop.Domain.Shops;
using Xunit;

namespace Ipstset.AzShop.Application.Tests.Shops
{
    public class UpdateShopHandlerShould
    {
        [Fact]
        public void Return_ShopResponse_Given_Valid_Request()
        {
            var repos = new MockShopRepositories();
            var existing = Shop.Load(Guid.NewGuid(), "oldname");
            repos.ShopRepository.SaveAsync(existing);

            var sut = new UpdateShopHandler(repos.ShopRepository);

            var request = new UpdateShopRequest
            {
                Id = existing.Id.ToString(),
                Name = "Test Shop",
                User = new AppUser { Roles = new[] { "admin" } }
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.IsType<ShopResponse>(actual);
            Assert.Equal(request.Id.ToString(), actual.Id);
            Assert.Equal(request.Name, actual.Name);
        }

        [Fact]
        public async void Throw_NotAuthorized_Given_NonAdmin_User()
        {
            var repos = new MockShopRepositories();
            var existing = Shop.Load(Guid.NewGuid(), "oldname");
            await repos.ShopRepository.SaveAsync(existing);

            var sut = new UpdateShopHandler(repos.ShopRepository);

            var request = new UpdateShopRequest
            {
                Id = existing.Id.ToString(),
                Name = "Test Shop",
                User = new AppUser()
            };

            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async Task Throw_NotFoundException_Given_Invalid_Id()
        {
            var repos = new MockShopRepositories();
            var sut = new UpdateShopHandler(repos.ShopRepository);
            var request = new UpdateShopRequest { Id = Guid.NewGuid().ToString(), User = new AppUser { Roles = new[] { "admin" } } };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
