using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Domain.Products.Events
{
    public class ProductCodeChanged : IEvent
    {
        public ProductCodeChanged(Product product)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            ProductId = product.Id;
            ProductCode = product.ProductCode;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
        public Guid ProductId { get; }
        public string ProductCode { get; }
    }
}
