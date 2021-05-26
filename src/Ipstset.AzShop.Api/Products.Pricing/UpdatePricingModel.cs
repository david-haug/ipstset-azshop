using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Domain.ValueObjects;

namespace Ipstset.AzShop.Api.Products.Pricing
{
    public class UpdatePricingModel
    {
        public IEnumerable<ProductPrice> Pricing { get; set; }
    }
}
