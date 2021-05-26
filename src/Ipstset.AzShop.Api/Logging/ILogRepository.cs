using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Api.Logging
{
    public interface ILogRepository
    {
        Task Save(RequestLog requestLog);
    }
}
