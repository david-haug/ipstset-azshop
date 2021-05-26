using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Domain.ValueObjects;

namespace Ipstset.AzShop.Api.Products.Options
{
    public class UpdateOptionsModel
    {
        public List<ProductOption> Options { get; set; }
    }
}
