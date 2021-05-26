using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Domain.ValueObjects
{
    public class ProductOptionDetail
    {
        public string Description { get; set; }
        public decimal AdditionalAmount { get; set; }
        public string UomText { get; set; }
    }
}
