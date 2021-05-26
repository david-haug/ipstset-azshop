using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.Shops.Events;

namespace Ipstset.AzShop.Domain.Shops
{
    public class Shop: Entity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public static Shop Create(string name)
        {
            var shop = Shop.Load(Guid.NewGuid(), name);
            shop.AddEvent(new ShopCreated(shop));
            return shop;
        }

        public static Shop Load(Guid id, string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name");

            var shop = new Shop
            {
                Id = id,
                Name = name
            };

            return shop;
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name");

            Name = name;
            AddEvent(new ShopNameChanged(this));
        }
    }
}
