using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Ipstset.AzShop.Application.Shops.GetShop
{
    public class GetShopRequest: IRequest<ShopResponse>
    {
        public string Id { get; set; }
        public AppUser User { get; set; }
    }
}
