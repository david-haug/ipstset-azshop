using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.ValueObjects;

namespace Ipstset.AzShop.Domain.Products.Events
{
    public class ProductCopyUpdated: IEvent
    {
        public ProductCopyUpdated(Product product)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            ProductId = product.Id;
            Copy = product.Copy;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
        public Guid ProductId { get; }
        public ProductCopy Copy { get; }
    }
}
