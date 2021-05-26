using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.Products;

namespace Ipstset.AzShop.Domain.Categories
{
    public class Category: Entity
    {
        public Guid Id { get; private set; }
        public Guid ShopId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

    }
}
