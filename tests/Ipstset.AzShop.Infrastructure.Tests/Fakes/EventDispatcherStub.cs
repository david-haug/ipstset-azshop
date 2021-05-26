using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.EventHandling;
using Ipstset.AzShop.Domain;

namespace Ipstset.AzShop.Infrastructure.Tests.Fakes
{
    public class EventDispatcherStub : IEventDispatcher
    {
        public Task DispatchAsync<T>(params T[] events) where T : IEvent
        {
            return Task.CompletedTask;
        }
    }
}
