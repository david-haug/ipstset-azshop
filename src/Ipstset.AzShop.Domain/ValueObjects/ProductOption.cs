using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Domain.ValueObjects
{
    public class ProductOption
    {
        public string Name { get; set; }
        public IEnumerable<ProductOptionDetail> Details { get; set; }
    }
}
