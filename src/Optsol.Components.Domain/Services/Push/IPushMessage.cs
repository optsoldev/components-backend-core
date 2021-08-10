using Optsol.Components.Domain.ValueObjects;
using System.Collections.Generic;

namespace Optsol.Components.Domain.Services.Push
{
    public interface IPushMessage
    {
        IEnumerable<ValueObject> GetPushMessages();
    }
}