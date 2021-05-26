using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Domain.Products;

namespace Ipstset.AzShop.Application.Specifications
{
    public class ProductCodeIsUniqueToShop
    {
        private IProductReadOnlyRepository _repository;
        public ProductCodeIsUniqueToShop(IProductReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public bool IsSatisifiedBy(Guid shopId, string productCode)
        {
            var product = _repository.GetByProductCodeAsync(productCode, shopId.ToString()).Result;
            return product == null;
        }

    }
}
