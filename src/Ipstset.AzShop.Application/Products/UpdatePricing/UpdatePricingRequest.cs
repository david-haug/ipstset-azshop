using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.ValueObjects;
using MediatR;

namespace Ipstset.AzShop.Application.Products.UpdatePricing
{
    public class UpdatePricingRequest: IRequest<ProductResponse>
    {
        public string ProductId { get; set; }
        public IEnumerable<ProductPrice> Pricing { get; set; }
        public AppUser User { get; set; }
    }
}
