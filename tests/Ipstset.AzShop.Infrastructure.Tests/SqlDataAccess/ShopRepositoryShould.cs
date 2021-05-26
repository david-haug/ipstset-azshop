using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Domain.Shops;
using Ipstset.AzShop.Infrastructure.SqlDataAccess;
using Ipstset.AzShop.Infrastructure.Tests.Fakes;
using Xunit;

namespace Ipstset.AzShop.Infrastructure.Tests.SqlDataAccess
{
    public class ShopRepositoryShould
    {
        [Fact]
        public async void DB_Add_New_Shop()
        {
            var sut = new ShopRepository(Settings.GetDbSettings(), new EventDispatcherStub());
            var shop = Shop.Load(Guid.NewGuid(), $"Unit test created shop {DateTime.Now}");
            await sut.SaveAsync(shop);

            var saved = await sut.GetAsync(shop.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async void DB_Return_Shop_Given_Valid_Id()
        {
            var shopId = Guid.NewGuid();
            await SaveNewShop(shopId);

            var sut = new ShopRepository(Settings.GetDbSettings(), new EventDispatcherStub());
            var actual = await sut.GetAsync(shopId);
            Assert.NotNull(actual);
            Assert.Equal(shopId, actual.Id);
        }

        private async Task SaveNewShop(Guid shopId)
        {
            var repo = new ShopRepository(Settings.GetDbSettings(), new EventDispatcherStub());
            var shop = Shop.Load(shopId, $"Unit test created shop {DateTime.Now}");
            await repo.SaveAsync(shop);
        }
    }
}
