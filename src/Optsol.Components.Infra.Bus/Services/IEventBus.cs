using Optsol.Components.Infra.Bus.Delegates;
using Optsol.Components.Infra.Bus.Events;

namespace Optsol.Components.Infra.Bus.Services
{
    public interface IEventBus
    {
        void Publish(IEvent @event);

        void Subscribe<TEvent>(ReceivedMessage received) where TEvent : IEvent;
    }
}
