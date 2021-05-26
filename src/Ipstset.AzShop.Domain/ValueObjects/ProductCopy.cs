using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Domain.ValueObjects
{
    public class ProductCopy
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}
