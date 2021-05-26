using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Api.Models
{
    public class ErrorModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public IEnumerable<ErrorDetailModel> Errors { get; set; }
    }
}
