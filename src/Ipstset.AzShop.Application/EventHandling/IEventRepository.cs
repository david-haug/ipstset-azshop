using Ipstset.AzShop.Application.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.AzShop.Application.EventHandling
{
    public interface IEventRepository
    {
        Task SaveAsync(ApplicationEvent @event);
        Task<QueryResult<ApplicationEvent>> GetEventsAsync(GetEventsRequest request);
    }
}
