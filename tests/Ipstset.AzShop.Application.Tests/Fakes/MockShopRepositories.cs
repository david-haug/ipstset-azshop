using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Shops;
using Ipstset.AzShop.Domain.Shops;

namespace Ipstset.AzShop.Application.Tests.Fakes
{
    public class MockShopRepositories
    {
        public MockShopRepositories()
        {
            _shops = new List<Shop>();
            ShopRepository = new Repository(_shops);
            ShopReadOnlyRepository = new ReadOnlyRepository(_shops);
        }

        public IShopRepository ShopRepository { get; set; }
        public IShopReadOnlyRepository ShopReadOnlyRepository { get; set; }

        private List<Shop> _shops;

        private class Repository : IShopRepository
        {
            private List<Shop> _shops;
            public Repository(List<Shop> shops)
            {
                _shops = shops;
            }
            public Task DeleteAsync(Shop shop)
            {
                throw new NotImplementedException();
            }

            public Task<Shop> GetAsync(Guid id)
            {
                var collection = _shops.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(collection);
            }

            public Task SaveAsync(Shop shop)
            {
                _shops.Add(shop);
                return Task.CompletedTask;
            }
        }

        private class ReadOnlyRepository : IShopReadOnlyRepository
        {
            private List<Shop> _shops;

            public ReadOnlyRepository(List<Shop> shops)
            {
                _shops = shops;
            }

            public async Task<ShopResponse> GetByIdAsync(string id)
            {
                var shop = _shops.FirstOrDefault(o => o.Id.ToString() == id);
                if (shop == null)
                    return null;

                var dto = new ShopResponse
                {
                    Id = shop.Id.ToString(),
                    Name = shop.Name
                };

                return dto;
            }

        }
    }
}
