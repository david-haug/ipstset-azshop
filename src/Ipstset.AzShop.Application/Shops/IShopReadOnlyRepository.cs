using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Application.Shops
{
    public interface IShopReadOnlyRepository
    {
        Task<ShopResponse> GetByIdAsync(string id);
    }
}
