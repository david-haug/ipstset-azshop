using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Attributes;
using Ipstset.AzShop.Application.Products.UpdatePricing;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.Products.Pricing
{
    [Route("products/{id}/pricing")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [HttpException]
    public class ProductsPricingController: BaseController
    {
        private readonly IMediator _mediator;
        public ProductsPricingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Update product pricing
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns>ProductResponse</returns>
        [HttpPut(Name = Constants.Routes.Products.UpdatePricing)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Patch([FromRoute] string id, [FromBody] UpdatePricingRequest request)
        {
            var result = await _mediator.Send(new UpdatePricingRequest
            {
                ProductId = id,
                Pricing = request.Pricing,
                User = AppUser
            });
            return Ok(result);
        }
    }
}
