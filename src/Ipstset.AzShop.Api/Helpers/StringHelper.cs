using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Api.Helpers
{
    public class StringHelper
    {
        /// <summary>
        /// Returns the path to an object's property as a camel-cased string, with each nested property camel-cased 
        /// </summary>
        /// <param name="path"></param>
        /// <example></example>
        /// <returns></returns>
        public static string CreateCamelCaseObjectPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path;

            //separate @ "."
            var formatted = "";
            foreach (var p in path.Split('.'))
            {
                var str = char.ToLowerInvariant(p[0]) + p.Substring(1);
                formatted += str + ".";
            }

            if (formatted.EndsWith("."))
                formatted = formatted.Remove(formatted.Length - 1);
            return formatted;
        }
    }
}
