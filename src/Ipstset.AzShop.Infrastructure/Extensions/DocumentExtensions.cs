using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Shops;
using Ipstset.AzShop.Infrastructure.Models;
using Ipstset.AzShop.Infrastructure.SqlDataAccess.Models;
using Newtonsoft.Json;

namespace Ipstset.AzShop.Infrastructure.Extensions
{
    public static class DocumentExtensions
    {
        public static ShopResponse ToShopResponse(this SqlDocument document)
        {
            try
            {
                //parse data
                var data = JsonConvert.DeserializeObject<ShopDocument>(document.Data);
                if (data != null)
                    return new ShopResponse
                    {
                        Id = data.Id,
                        Name = data.Name
                    };

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ProductResponse ToProductResponse(this SqlDocument document)
        {
            try
            {
                //parse data
                var data = JsonConvert.DeserializeObject<ProductDocument>(document.Data);
                if (data != null)
                    return new ProductResponse
                    {
                        Id = data.Id,
                        ShopId = data.ShopId,
                        ProductCode = data.ProductCode,
                        Type = data.Type,
                        Copy = data.Copy,
                        IsActive = data.IsActive,
                        Pricing = data.Pricing,
                        Options = data.Options
                    };

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
