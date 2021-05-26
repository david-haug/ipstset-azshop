using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Domain.Products.Events
{
    public class ProductTypeChanged : IEvent
    {
        public ProductTypeChanged(Product product)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            ProductId = product.Id;
            Type = product.Type;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
        public Guid ProductId { get; }
        public string Type { get; }
    }
}
