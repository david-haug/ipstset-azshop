using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application;
using Ipstset.AzShop.Application.EventHandling;
using Ipstset.AzShop.Application.Exceptions;
using MediatR;

namespace Ipstset.AzShop.Application.Events.GetEvents
{
    public class GetEventsHandler : IRequestHandler<GetEventsRequest, QueryResult<ApplicationEvent>>
    {
        private IEventRepository _repository;
        public GetEventsHandler(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult<ApplicationEvent>> Handle(GetEventsRequest request, CancellationToken cancellationToken)
        {
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            return await _repository.GetEventsAsync(request);
        }
    }
}
