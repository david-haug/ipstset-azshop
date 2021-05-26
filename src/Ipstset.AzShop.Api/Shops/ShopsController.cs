using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.Auth.JwtTokens.Attributes;
using Ipstset.AzShop.Api.Attributes;
using Ipstset.AzShop.Application.Shops;
using Ipstset.AzShop.Application.Shops.CreateShop;
using Ipstset.AzShop.Application.Shops.GetShop;
using Ipstset.AzShop.Application.Shops.UpdateShop;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.Shops
{
    [Route("shops")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [HttpException]
    public class ShopsController: BaseController
    {
        private readonly IMediator _mediator;
        public ShopsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get shop by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = Constants.Routes.Shops.GetShop)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [NoAuthentication(Order = int.MinValue)]
        public async Task<ActionResult<ShopResponse>> Get([FromRoute] string id) => await _mediator.Send(new GetShopRequest { Id = id, User = AppUser });

        ///// <summary>
        ///// Gets all shops matching supplied criteria
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<QueryResult<ShopResponse>>> Get([FromQuery] SearchMcuActorsModel model)
        //{
        //    return await _mediator.Send(new SearchMcuActorsRequest
        //    {
        //        Name = model.Name,
        //        Limit = model.Limit ?? Constants.MaxRequestLimit,
        //        Sort = model.Sort.ToSortItems("FullName")
        //    });
        //}

        /// <summary>
        /// Create shop
        /// </summary>
        /// <param name="request">CreateShopModel</param>
        /// <returns></returns>
        [HttpPost(Name = Constants.Routes.Shops.CreateShop)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ShopResponse>> Post([FromBody] CreateShopModel request)
        {
            var result = await _mediator.Send(new CreateShopRequest
            {
                Name = request.Name,
                User = AppUser
            });

            return CreatedAtRoute(Constants.Routes.Shops.GetShop, new { result.Id }, result);
        }

        /// <summary>
        /// Update shop
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request">UpdateShopModel</param>
        /// <returns></returns>
        [HttpPut("{id}", Name = Constants.Routes.Shops.UpdateShop)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ShopResponse>> Put([FromRoute] string id, [FromBody] UpdateShopModel request)
        {
            var result = await _mediator.Send(new UpdateShopRequest
            {
                Id = id,
                Name = request.Name,
                User = AppUser
            });

            return Ok(result);
        }
    }
}
