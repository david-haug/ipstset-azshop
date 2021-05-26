using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.Auth.JwtTokens;
using Ipstset.Auth.JwtTokens.Attributes;
using Ipstset.AzShop.Api.Attributes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.Tokens
{
    [Route("tokens")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [HttpException]
    public class TokensController: BaseController
    {
        private IJwtTokenManager _tokenManager;
        public TokensController(IJwtTokenManager jwtTokenManager)
        {
            _tokenManager = jwtTokenManager;
        }
        /// <summary>
        /// Create api token
        /// </summary>
        /// <returns></returns>
        [NoAuthentication(Order = int.MinValue)]
        [HttpPost(Name = Constants.Routes.Tokens.CreateToken)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] CreateTokenModel request)
        {
            string userId = null;
            string userName = null;
            string[] roles;

            if (string.IsNullOrEmpty(request.ClientId) && !string.IsNullOrWhiteSpace(request.UserName) && request.UserName.ToLower() == "jtester" && request.Password == "testm3")
            {
                userId = "e4116e94-6ef4-4448-b193-34d568f70079";
                userName = "jtester";
                roles = new[] {"admin"};
            }
            else if (request.ClientId == "dev_client")
            {
                userId = "94c99596-840b-4d37-80ca-86adc9020498";
                userName = "anonymous";
                roles = new[] { "readonly_94bb7049-b8af-4bca-b4c4-795c3d5065ad" };
            }
            else
            {
                return BadRequest();
            }

            var claims = TokenClaimsService.Create(userId, userName, roles);
            var token = _tokenManager.CreateToken(claims);
            return Ok(new {token});
        }
    }
}
