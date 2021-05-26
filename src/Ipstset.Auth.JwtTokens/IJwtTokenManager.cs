using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ipstset.Auth.JwtTokens
{
    public interface IJwtTokenManager
    {
        string CreateToken(IEnumerable<Claim> claims);
        bool ValidateToken(string token);
        JwtSecurityToken ReadToken(string token);
        ClaimsPrincipal CreateClaimsPrincipal(string token);
    }
}
