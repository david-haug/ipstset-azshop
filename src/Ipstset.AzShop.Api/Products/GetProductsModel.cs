using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Models;
using Ipstset.AzShop.Domain.Products;

namespace Ipstset.AzShop.Api.Products
{
    public class GetProductsModel: QueryModel
    {
        public string ShopId { get; set; }
        public string Type { get; set; }
        public bool? IsActive { get; set; }
    }
}
