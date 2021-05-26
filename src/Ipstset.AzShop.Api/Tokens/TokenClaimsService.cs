using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Ipstset.AzShop.Application;

namespace Ipstset.AzShop.Api.Tokens
{
    public class TokenClaimsService
    {
        public static IEnumerable<Claim> Create(string userId, string userName, string [] roles)
        {
            //map claims here
            return new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, userId),
                new Claim(JwtClaimTypes.PreferredUserName, userName),
                //new Claim(JwtClaimTypes.GivenName, user.FirstName ?? ""),
                //new Claim(JwtClaimTypes.FamilyName, user.LastName ?? ""),
                //new Claim(JwtClaimTypes.Email, user.Email ?? ""),
                new Claim("roles",string.Join(",", roles)),
                new Claim(JwtClaimTypes.SessionId, Guid.NewGuid().ToString("N"))
            };
        }

        public static AppUser CreateAppUser(IEnumerable<Claim> claims)
        {
            var claimList = claims.ToList();

            //var clientId = claimList.FirstOrDefault(c => c.Type == "client_id")?.Value;
            //if (clientId == Constants.ClientCredentialsClientIdForSystemUser)
            //{
            //    return new AppUser
            //    {
            //        UserId = Constants.SystemUserId,
            //        Roles = new[] { "admin", "user" }
            //    };
            //}

            return new AppUser
            {
                UserId = claimList.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value,
                Roles = claimList.FirstOrDefault(c => c.Type == "roles")?.Value.Split(",")
            };
        }
    }
}
