using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Application.Helpers
{
    public class ValidationHelper
    {
        public static bool ValidateId(string id)
        {
            return Guid.TryParse(id, out var result);
        }

    }
}
