using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Application.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisifiedBy(T entity);
    }
}
