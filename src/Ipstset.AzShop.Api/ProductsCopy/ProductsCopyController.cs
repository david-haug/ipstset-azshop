using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Attributes;
using Ipstset.AzShop.Application.Products.UpdateCopy;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.ProductsCopy
{
    [Route("products/{id}/copy")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [HttpException]
    public class ProductsCopyController : BaseController
    {
        private readonly IMediator _mediator;
        public ProductsCopyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Update product copy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns>ProductResponse</returns>
        [HttpPut(Name = Constants.Routes.Products.UpdateCopy)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Patch([FromRoute] string id, [FromBody] UpdateCopyRequest request)
        {
            var result = await _mediator.Send(new UpdateCopyRequest
            {
                ProductId = id, 
                Title = request.Title,
                Description = request.Description,
                ShortDescription = request.ShortDescription,
                ThumbnailUrl = request.ThumbnailUrl,
                User = AppUser
            });
            return Ok(result);
        }
    }
}
