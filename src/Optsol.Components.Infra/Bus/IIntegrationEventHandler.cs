using System.Threading.Tasks;

namespace Optsol.Components.Infra.Bus
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEvent
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEvent 
    {

    }
}