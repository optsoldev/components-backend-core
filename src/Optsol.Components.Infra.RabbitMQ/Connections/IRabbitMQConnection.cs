using RabbitMQ.Client;

namespace Optsol.Components.Infra.RabbitMQ.Connections
{
    public interface IRabbitMQConnection
    {
        bool Connected { get; }

        bool Disconnected { get; }

        void Connect();

        IModel CreateModel();
    }
}
