using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.ValueObjects;
using MediatR;

namespace Ipstset.AzShop.Application.Products.UpdateOptions
{
    public class UpdateOptionsRequest: IRequest<ProductResponse>
    {
        public string ProductId { get; set; }
        public IEnumerable<ProductOption> Options { get; set; }
        public AppUser User { get; set; }
    }
}
