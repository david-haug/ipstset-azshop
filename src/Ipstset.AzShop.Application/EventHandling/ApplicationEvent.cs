using Ipstset.AzShop.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Application;

namespace Ipstset.AzShop.Application.EventHandling
{
    public class ApplicationEvent
    {
        public ApplicationEvent()
        {

        }

        public ApplicationEvent(IEvent @event, AppUser appUser)
        {
            Id = @event.Id.ToString();
            DateOccurred = @event.DateOccurred;
            Name = @event.GetType().Name;
            Event = @event;
            User = appUser;
        }

        public string Id { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
        public string Name { get; set; }
        public object Event { get; set; }
        public AppUser User { get; set; }
    }
}
