using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Shops.GetShop;
using Ipstset.AzShop.Application.Tests.Fakes;
using Ipstset.AzShop.Domain.Shops;
using Xunit;

namespace Ipstset.AzShop.Application.Tests.Shops
{
    public class GetShopHandlerShould
    {
        [Fact]
        public async void Return_ShopResponse_given_valid_id()
        {
            var repos = new MockShopRepositories();
            var shop = Shop.Load(Guid.NewGuid(), "name");
            await repos.ShopRepository.SaveAsync(shop);

            var sut = new GetShopHandler(repos.ShopReadOnlyRepository);
            var request = new GetShopRequest { Id = shop.Id.ToString(), User= new AppUser { Roles = new[] { $"readonly_{shop.Id}" } } };
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.Equal(request.Id, actual.Id);
        }

        [Fact]
        public async Task Throw_NotFoundException_given_invalid_id()
        {
            var repos = new MockShopRepositories();
            var sut = new GetShopHandler(repos.ShopReadOnlyRepository);
            var request = new GetShopRequest { Id = Guid.NewGuid().ToString(), User = new AppUser { Roles = new[] { "admin" } } };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        //[Fact]
        //public async Task Throw_NotAuthorizedException_if_user_does_not_have_access()
        //{
        //    var repos = new MockShopRepositories();
        //    var shop = Shop.Load(Guid.NewGuid(), "name");
        //    await repos.ShopRepository.SaveAsync(shop);

        //    var sut = new GetShopHandler(repos.ShopReadOnlyRepository);
        //    var request = new GetShopRequest { Id = shop.Id.ToString(), User = new AppUser() };
        //    await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        //}

    }
}
