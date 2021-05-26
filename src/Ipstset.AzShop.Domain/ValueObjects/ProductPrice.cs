using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Domain.ValueObjects
{
    public class ProductPrice
    {
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }
        public double Amount { get; set; }
        public string UomText { get; set; }

    }
}
