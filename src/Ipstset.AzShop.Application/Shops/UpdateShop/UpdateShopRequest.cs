using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Ipstset.AzShop.Application.Shops.UpdateShop
{
    public class UpdateShopRequest: IRequest<ShopResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AppUser User { get; set; }
    }
}
