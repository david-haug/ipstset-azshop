using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Shops;
using Ipstset.AzShop.Domain.Shops;
using Ipstset.AzShop.Infrastructure.SqlDataAccess;
using Ipstset.AzShop.Infrastructure.Tests.Fakes;
using Xunit;
namespace Ipstset.AzShop.Infrastructure.Tests.SqlDataAccess
{
    public class ShopReadOnlyRepositoryShould
    {
        [Fact]
        public async void DB_Return_ShopResponse_Given_Valid_Id()
        {
            var id = Guid.NewGuid();
            await SaveNewShop(id);

            var sut = new ShopReadOnlyRepository(Settings.GetDbSettings());
            var shop = await sut.GetByIdAsync(id.ToString());
            Assert.NotNull(shop);
            Assert.Equal(id.ToString(), shop.Id);
            Assert.IsType<ShopResponse>(shop);
        }

        private async Task SaveNewShop(Guid shopId)
        {
            var repo = new ShopRepository(Settings.GetDbSettings(), new EventDispatcherStub());
            var shop = Shop.Load(shopId, $"Unit test created shop {DateTime.Now}");
            await repo.SaveAsync(shop);
        }
    }
}
