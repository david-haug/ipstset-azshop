using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Api.ProductsCopy
{
    public class UpdateCopyModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
