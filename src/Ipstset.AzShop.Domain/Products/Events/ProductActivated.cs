using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Domain.Products.Events
{
    public class ProductActivated: IEvent
    {
        public ProductActivated(Product product)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            ProductId = product.Id;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
        public Guid ProductId { get; }
    }
}
