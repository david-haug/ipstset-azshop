using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;
using MediatR;

namespace Ipstset.AzShop.Application.Products.CreateProduct
{
    public class CreateProductRequest: IRequest<ProductResponse>
    {
        public string ShopId { get; set; }
        public string ProductCode { get; set; }
        public string Type { get; set; }
        public ProductCopy Copy { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<ProductPrice> Pricing { get; set; }
        public IEnumerable<ProductOption> Options { get; set; }
        public AppUser User { get; set; }
    }
}
