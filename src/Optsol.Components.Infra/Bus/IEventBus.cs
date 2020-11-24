namespace Optsol.Components.Infra.Bus
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);

        void Subscribe<TEvent, TEventHandler>()
            where TEvent: IntegrationEvent
            where TEventHandler: IIntegrationEventHandler<TEvent>;

        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}
