using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Tokens;
using Ipstset.AzShop.Application;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api
{
    public class BaseController : Controller
    {
        public AppUser AppUser => TokenClaimsService.CreateAppUser(User.Claims);
    }
}
