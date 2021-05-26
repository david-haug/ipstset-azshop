using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Ipstset.AzShop.Application.Shops.CreateShop
{
    public class CreateShopRequest: IRequest<ShopResponse>
    {
        public string Name { get; set; }
        public AppUser User { get; set; }
    }
}
