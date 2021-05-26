using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ipstset.Auth.JwtTokens
{
    public class JwtTokenManager: IJwtTokenManager
    {
        private JwtTokenSettings _settings;

        public JwtTokenManager(JwtTokenSettings settings)
        {
            _settings = settings;
        }

        public string CreateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuers.ToList().First(),
                audience: _settings.Audiences.ToList().First(),
                claims: claims,
                expires: GetTokenExpireDate(),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var param = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuers = _settings.Issuers,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidAudiences = _settings.Audiences,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
                    ValidateLifetime = true
                };
                handler.ValidateToken(token, param, out var validatedToken);
                return true;
            }
            catch (SecurityTokenException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public JwtSecurityToken ReadToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        public ClaimsPrincipal CreateClaimsPrincipal(string token)
        {
            var jwt = ReadToken(token);
            var identity = new ClaimsIdentity(jwt.Claims);
            return new ClaimsPrincipal(identity);
        }

        private DateTime GetTokenExpireDate()
        {
            return DateTime.UtcNow.AddMinutes(_settings.MinutesToExpire);
        }
    }
}
