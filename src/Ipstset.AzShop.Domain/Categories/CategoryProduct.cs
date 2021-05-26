using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Domain.Products;

namespace Ipstset.AzShop.Domain.Categories
{
    public class CategoryProduct
    {
        public Guid CategoryId { get; set; }
        public Product Product { get; set; }
        public int SortOrder { get; set; }
    }
}
