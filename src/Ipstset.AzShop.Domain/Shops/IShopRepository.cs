using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Domain.Shops
{
    public interface IShopRepository
    {
        Task<Shop> GetAsync(Guid id);
        Task SaveAsync(Shop shop);
        Task DeleteAsync(Shop shop);
    }
}
