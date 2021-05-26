using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ipstset.Auth.JwtTokens.Attributes
{
    public class NoAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items[Constants.HttpContextItems.NoAuthenticationKey] = true;
            base.OnActionExecuting(context);
        }
    }
}
