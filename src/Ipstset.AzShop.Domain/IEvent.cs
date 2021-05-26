using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Domain
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTimeOffset DateOccurred { get; }
    }
}
