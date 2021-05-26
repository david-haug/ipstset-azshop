using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Api.Models
{
    public class QueryModel
    {
        public int? Limit { get; set; }
        public string StartAfter { get; set; }
        public string[] Sort { get; set; }
    }
}
