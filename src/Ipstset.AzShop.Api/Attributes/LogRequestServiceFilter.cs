using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Ipstset.AzShop.Api.Attributes
{
    public class LogRequestServiceFilter : IActionFilter
    {
        private ILogRepository _logRepository;

        public LogRequestServiceFilter(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Log(context.RouteData, context.HttpContext);
        }

        private async Task Log(RouteData routeData, HttpContext context)
        {
            try
            {
                await _logRepository.Save(RequestLog.Create(routeData,context));
            }
            catch(Exception ex)
            {
                //swallow for now
            }

        }
    }
}
