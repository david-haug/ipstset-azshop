using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.ValueObjects;

namespace Ipstset.AzShop.Domain.Products.Events
{
    public class ProductPricingUpdated:IEvent
    {
        public ProductPricingUpdated(Product product)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            ProductId = product.Id;
            Pricing = product.Pricing;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
        public Guid ProductId { get; }
        public IEnumerable<ProductPrice> Pricing { get; }
    }
}
