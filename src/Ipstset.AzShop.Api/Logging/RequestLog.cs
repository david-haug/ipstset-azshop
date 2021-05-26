
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Api.Logging
{
    public class RequestLog
    {
        public DateTime LogDate { get; set; }
        public dynamic Parameters { get; set; }
        public string Route { get; set; }

        public static RequestLog Create(RouteData routeData, HttpContext context)
        {
            //can't get session if logging before auth
            //var sessionId = context.User?.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();

            var parameters = new
            {
                //sessionId,
                connection = new
                {
                    id = context.Connection.Id.ToString(),
                    localIpAddress = context.Connection.LocalIpAddress.ToString(),
                    localPort = context.Connection.LocalPort.ToString(),
                    remoteIpAddress = context.Connection.RemoteIpAddress.ToString(),
                    remotePort = context.Connection.RemotePort.ToString()
                },
                route = routeData.Values.ToDictionary(r => r.Key, r => r.Value.ToString(), StringComparer.OrdinalIgnoreCase),
                host = context.Request.Host.ToString(),
                headers = context.Request.Headers.Where(h => !string.IsNullOrEmpty(h.Value)),
                path = context.Request.Path.ToString(),
                queryString = context.Request.QueryString.ToString()
            };

            return new RequestLog
            {
                LogDate = DateTime.Now,
                Parameters = parameters,
                Route = GetAbsoluteUri(context).ToString()
            };
        }

        private static Uri GetAbsoluteUri(HttpContext context)
        {
            var request = context.Request;
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };

            return uriBuilder.Uri;
        }
    }
}
