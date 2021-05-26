using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Domain.Products
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(Guid id);
        Task SaveAsync(Product product);
        Task DeleteAsync(Product product);
    }
}

