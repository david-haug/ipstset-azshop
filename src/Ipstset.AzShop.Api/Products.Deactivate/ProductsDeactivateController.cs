using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Attributes;
using Ipstset.AzShop.Application.Products.DeactivateProduct;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.Products.Deactivate
{
    [Route("products/{id}/deactivate")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [HttpException]
    public class ProductsDeactivateController : BaseController
    {
        private readonly IMediator _mediator;
        public ProductsDeactivateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Deactivate product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProductResponse</returns>
        [HttpPatch(Name = Constants.Routes.Products.DeactivateProduct)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Patch([FromRoute] string id)
        {
            var result = await _mediator.Send(new DeactivateProductRequest { ProductId = id, User = AppUser });
            return Ok(result);
        }
    }
}
