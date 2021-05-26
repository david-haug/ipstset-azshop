using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Domain.Products;

namespace Ipstset.AzShop.Application.Extensions
{
    public static class ProductExtensions
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            try
            {
                return new ProductResponse
                {
                    Id = product.Id.ToString(),
                    ShopId = product.ShopId.ToString(),
                    ProductCode = product.ProductCode,
                    Type = product.Type,
                    Copy = product.Copy,
                    IsActive = product.IsActive,
                    Pricing = product.Pricing,
                    Options = product.Options
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
