using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.Products;
using MediatR;

namespace Ipstset.AzShop.Application.Products.GetProducts
{
    public class GetProductsRequest:IRequest<QueryResult<ProductResponse>>
    {
        public string ShopId { get; set; }
        public string Type { get; set; }
        public bool? IsActive { get; set; }
        
        public AppUser User { get; set; }
        public int Limit { get; set; }
        public string StartAfter { get; set; }
        public IEnumerable<SortItem> Sort { get; set; }
    }
}
