using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;

namespace Ipstset.AzShop.Api.Products
{
    public class CreateProductModel
    {
        public string ShopId { get; set; }
        public string ProductCode { get; set; }
        public string Type { get; set; }
        public ProductCopy Copy { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<ProductPrice> Pricing { get; set; }
        public IEnumerable<ProductOption> Options { get; set; }
    }
}
