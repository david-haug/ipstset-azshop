using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Ipstset.AzShop.Application.Products.ChangeProductCode
{
    public class ChangeProductCodeRequest: IRequest<ProductResponse>
    { 
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public AppUser User { get; set; }
    }
}
