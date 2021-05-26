using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Ipstset.AzShop.Application.Products.ActivateProduct
{
    public class ActivateProductRequest:IRequest<ProductResponse>
    {
        public string ProductId { get; set; }
        public AppUser User { get; set; }
    }
}
