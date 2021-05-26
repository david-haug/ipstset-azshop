using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Ipstset.Auth.JwtTokens.Attributes
{
    public class AuthenticateJwtTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.HttpContext.Items.ContainsKey(Constants.HttpContextItems.NoAuthenticationKey))
            {
                bool.TryParse(actionContext.HttpContext.Items[Constants.HttpContextItems.NoAuthenticationKey].ToString(), out bool noAuthentication);
                if (noAuthentication)
                    return;
            }

            var tokenManager = actionContext.HttpContext.RequestServices.GetService<IJwtTokenManager>();

            var request = actionContext.HttpContext.Request;
            var authorization = request.Headers["Authorization"].ToString();
            string token;
            if (!string.IsNullOrEmpty(authorization))
            {
                token = authorization.Replace("Bearer ", "");
            }
            else
            {
                //using ObjectResult instead of to control info returned in response
                actionContext.Result = _unauthorizedResult;
                return;
            }

            var valid = tokenManager.ValidateToken(token);
            if (valid)
            {
                actionContext.HttpContext.User = tokenManager.CreateClaimsPrincipal(token);
                //todo: check claims or scope vs requested endpoint...return Unauthorized if no access
                //[AuthenticateJwtToken(Scopes="read-product write-product")
            }
            else
            {
                actionContext.Result = _unauthorizedResult;
            }
        }

        private readonly ObjectResult _unauthorizedResult = new ObjectResult(new
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Message = "Unauthorized."
            })
            { StatusCode = (int)HttpStatusCode.Unauthorized };

    }
}
