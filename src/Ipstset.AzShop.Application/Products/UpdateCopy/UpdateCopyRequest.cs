using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MediatR;

namespace Ipstset.AzShop.Application.Products.UpdateCopy
{
    public class UpdateCopyRequest: IRequest<ProductResponse>
    {
        public string ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string ThumbnailUrl { get; set; }
        public AppUser User { get; set; }
    }
}
