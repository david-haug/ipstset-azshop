using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.ValueObjects;

namespace Ipstset.AzShop.Domain.Products.Events
{
    public class ProductOptionsUpdated : IEvent
    {
        public ProductOptionsUpdated(Product product)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            ProductId = product.Id;
            Options = product.Options;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
        public Guid ProductId { get; }
        public IEnumerable<ProductOption> Options { get; }
    }
}
