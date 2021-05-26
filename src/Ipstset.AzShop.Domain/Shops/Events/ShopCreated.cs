using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ipstset.AzShop.Domain.Shops.Events
{
    public class ShopCreated:IEvent
    {
        public ShopCreated(Shop shop)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            ShopId = shop.Id;
            Name = shop.Name;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
        public Guid ShopId { get; }
        public string Name { get; }
    }
}
