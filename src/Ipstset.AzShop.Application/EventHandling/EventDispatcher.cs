using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Domain;

namespace Ipstset.AzShop.Application.EventHandling
{
    public class EventDispatcher: IEventDispatcher
    {
        private IEventRepository _repository;
        private IServiceProvider _serviceProvider;
        public EventDispatcher(IEventRepository repository, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync<T>(params T[] events) where T : Domain.IEvent
        {
            foreach (var @event in events)
            {
                if (@event == null)
                    throw new ArgumentNullException(nameof(@event), "Event cannot be null");

                var appUser = _serviceProvider.GetService(typeof(AppUser)) as AppUser;
                await _repository.SaveAsync(new ApplicationEvent(@event, appUser));

                var eventType = @event.GetType();
                var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
                var handler = _serviceProvider.GetService(handlerType);

                if (handler == null)
                    continue;

                var method = handler.GetType()
                    .GetRuntimeMethods()
                    .First(x => x.Name.Equals("HandleAsync"));

                await (Task)method.Invoke(handler, new object[] { @event });
            }
        }
    }
}
