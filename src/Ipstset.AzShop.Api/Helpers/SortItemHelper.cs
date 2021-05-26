using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Application;

namespace Ipstset.AzShop.Api.Helpers
{
    public static class ArrayExtensions
    {
        public static IEnumerable<SortItem> ToSortItems(this string[] sort, string defaultSortByName, bool isDescending = false)
        {
            var sortItems = new List<SortItem>();
            if (sort == null || !sort.Any())
            {
                sortItems.Add(new SortItem { Name = defaultSortByName, IsDescending = isDescending });
            }
            else
            {
                foreach (var s in sort)
                {
                    var name = s;
                    var descending = false;
                    if (s.StartsWith("-"))
                    {
                        name = s.Substring(1, s.Length - 1);
                        descending = true;
                    }

                    sortItems.Add(new SortItem { Name = name, IsDescending = descending });
                }
            }

            return sortItems;
        }

    }
}
