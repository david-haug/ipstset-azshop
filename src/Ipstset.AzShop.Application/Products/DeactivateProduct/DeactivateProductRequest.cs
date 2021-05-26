using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Ipstset.AzShop.Application.Products.DeactivateProduct
{
    public class DeactivateProductRequest: IRequest<ProductResponse>
    {
        public string ProductId { get; set; }
        public AppUser User { get; set; }
    }
}
