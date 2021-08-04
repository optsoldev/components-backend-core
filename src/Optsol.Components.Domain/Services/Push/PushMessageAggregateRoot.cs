using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.ValueObjects;
using System.Collections.Generic;

namespace Optsol.Components.Domain.Services.Push
{
    public abstract class PushMessageAggregateRoot : AggregateRoot
    {
        public abstract IEnumerable<ValueObject> GetPushMessages();
    }
}
