using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;

namespace Ipstset.AzShop.Application.Products
{
    public class ProductResponse
    {
        public string Id { get; set; }
        public string ShopId { get; set; }
        public string ProductCode { get; set; }
        public string Type { get; set; }
        public ProductCopy Copy { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<ProductPrice> Pricing { get; set; }
        public IEnumerable<ProductOption> Options { get; set; }
    }
}
