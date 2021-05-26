using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Products.GetProducts;

namespace Ipstset.AzShop.Application.Products
{
    public interface IProductReadOnlyRepository
    {
        Task<ProductResponse> GetByIdAsync(string id);
        Task<QueryResult<ProductResponse>> GetProductsAsync(GetProductsRequest request);
        Task<ProductResponse> GetByProductCodeAsync(string productCode, string shopId);
    }
}
