using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Ipstset.AzShop.Application.Products.GetProduct
{
    public class GetProductRequest: IRequest<ProductResponse>
    {
        public string Id { get; set; }
        public AppUser User { get; set; }
    }
}
