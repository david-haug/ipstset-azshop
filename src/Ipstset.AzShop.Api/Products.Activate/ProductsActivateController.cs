using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Attributes;
using Ipstset.AzShop.Application.Products.ActivateProduct;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.Products.Activate
{
    [Route("products/{id}/activate")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [HttpException]
    public class ProductsActivateController : BaseController
    {
        private readonly IMediator _mediator;
        public ProductsActivateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Activate product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProductResponse</returns>
        [HttpPatch(Name = Constants.Routes.Products.ActivateProduct)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Patch([FromRoute] string id)
        {
            var result = await _mediator.Send(new ActivateProductRequest { ProductId = id, User = AppUser });
            return Ok(result);
        }
    }
}
