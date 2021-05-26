using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Attributes;
using Ipstset.AzShop.Api.Helpers;
using Ipstset.AzShop.Application;
using Ipstset.AzShop.Application.EventHandling;
using Ipstset.AzShop.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.Events
{
    [Route("events")]
    [ApiController]
    [Produces("application/json")]
    [HttpException]
    public class EventsController : BaseController
    {
        private readonly IMediator _mediator;
        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all events matching supplied criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<QueryResult<ApplicationEvent>> Get([FromQuery] GetEventsModel request)
        {
            return await _mediator.Send(new GetEventsRequest
            {
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Limit = request.Limit ?? Constants.MaxRequestLimit,
                StartAfter = request.StartAfter,
                User = AppUser,
                Sort = request.Sort.ToSortItems("DateOccurred")
            });
        }
    }
}
