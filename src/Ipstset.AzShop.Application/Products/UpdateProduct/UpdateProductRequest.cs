using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Ipstset.AzShop.Application.Products.UpdateProduct
{
    public class UpdateProductRequest: IRequest<ProductResponse>
    {
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Type { get; set; }
        public AppUser User { get; set; }
    }
}
