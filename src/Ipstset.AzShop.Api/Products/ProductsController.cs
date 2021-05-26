using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.Auth.JwtTokens.Attributes;
using Ipstset.AzShop.Api.Attributes;
using Ipstset.AzShop.Api.Helpers;
using Ipstset.AzShop.Application;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Products.ChangeProductCode;
using Ipstset.AzShop.Application.Products.CreateProduct;
using Ipstset.AzShop.Application.Products.GetProduct;
using Ipstset.AzShop.Application.Products.GetProducts;
using Ipstset.AzShop.Application.Products.UpdateProduct;
using Ipstset.AzShop.Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.Products
{
    [Route("products")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [HttpException]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all products matching supplied criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [NoAuthentication(Order = int.MinValue)]
        public async Task<ActionResult<QueryResult<ProductResponse>>> Get([FromQuery] GetProductsModel model)
        {
            return await _mediator.Send(new GetProductsRequest
            {
                ShopId = model.ShopId,
                Type = model.Type,
                IsActive = model.IsActive,
                Limit = model.Limit ?? Constants.MaxRequestLimit,
                StartAfter = model.StartAfter,
                Sort = model.Sort.ToSortItems("Id"),
                User = AppUser
            });
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = Constants.Routes.Products.GetProduct)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [NoAuthentication(Order = int.MinValue)]
        public async Task<ActionResult<ProductResponse>> Get([FromRoute] string id) => await _mediator.Send(new GetProductRequest { Id = id, User = AppUser });

        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="request">CreateProductModel</param>
        /// <returns></returns>
        [HttpPost(Name = Constants.Routes.Products.CreateProduct)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductResponse>> Post([FromBody] CreateProductModel request)
        {
            var result = await _mediator.Send(new CreateProductRequest
            {
                ShopId = request.ShopId,
                ProductCode = request.ProductCode,
                Type = request.Type,
                Copy = request.Copy,
                IsActive = request.IsActive,
                Pricing = request.Pricing,
                Options = request.Options,
                User = AppUser
            });

            return CreatedAtRoute(Constants.Routes.Products.GetProduct, new { result.Id }, result);
        }

        /// <summary>
        /// Updates a product's ProductCode and/or Type
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="request">UpdateProductModel</param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = Constants.Routes.Products.UpdateProduct)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Patch([FromRoute] string id, [FromBody] UpdateProductModel request)
        {
            var result = await _mediator.Send(new UpdateProductRequest
            {
                ProductId = id,
                ProductCode = request.ProductCode,
                Type = request.Type,
                User = AppUser
            });

            return Ok(result);
        }
    }
}
