using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Attributes;
using Ipstset.AzShop.Application.Products.UpdateOptions;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.Products.Options
{
    [Route("products/{id}/options")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [HttpException]
    public class ProductsOptionsController : BaseController
    {
        private readonly IMediator _mediator;
        public ProductsOptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Update product options
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns>ProductResponse</returns>
        [HttpPut(Name = Constants.Routes.Products.UpdateOptions)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Patch([FromRoute] string id, [FromBody] UpdateOptionsRequest request)
        {
            var result = await _mediator.Send(new UpdateOptionsRequest
            {
                ProductId = id,
                Options = request.Options,
                User = AppUser
            });
            return Ok(result);
        }
    }
}
