using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Domain
{
    public interface IEntity
    {
        void AddEvent(IEvent @event);
        IReadOnlyCollection<IEvent> DequeueEvents();

        IReadOnlyCollection<IEvent> Events { get; }
    }
}
