using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Bus.Delegates;
using Optsol.Components.Infra.Bus.Events;
using Optsol.Components.Infra.Bus.Services;
using Optsol.Components.Infra.RabbitMQ.Connections;
using Optsol.Components.Shared.Settings;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;

namespace Optsol.Components.Infra.RabbitMQ.Services
{
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly ILogger _logger;
        private readonly IRabbitMQConnection _connection;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly string[] IgnoredProperties = new[] { "notifications", "isvalid" };

        public EventBusRabbitMQ(RabbitMQSettings rabbitMQSettings, ILoggerFactory logger, IRabbitMQConnection connection)
        {
            _logger = logger?.CreateLogger<EventBusRabbitMQ>();
            _logger?.LogInformation("Inicializando EventBusRabbitMQ");

            _connection = connection ?? throw new ArgumentNullException(nameof(connection));

            _rabbitMQSettings = rabbitMQSettings ?? throw new ArgumentException(nameof(rabbitMQSettings));
        }

        public void Publish(IEvent @event)
        {
            var eventName = @event.GetType().Name;

            _logger?.LogInformation($"Executando { nameof(Publish) }({eventName})");

            if (_connection.Disconnected)
            {
                _connection.Connect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(3, attemp => TimeSpan.FromSeconds(Math.Pow(2, attemp)), (ex, time) =>
                {
                    _logger?.LogWarning("Não foi possível publicar a '{EventName}': ({ExceptionMessage})", eventName, ex.Message);
                });


            using (var channel = _connection.CreateModel())
            {
                DeclareExchange(channel);

                DeclareQueueAndBind(eventName, channel);

                policy.Execute(() =>
                {
                    _logger?.LogInformation("Publicando '{EventName}': {Event}", eventName, @event.ToJson());

                    channel.BasicPublish(
                            exchange: _rabbitMQSettings.ExchangeName,
                            routingKey: eventName,
                            basicProperties: null,
                            body: @event.ToJson(IgnoredProperties).ToBytes()
                        );
                });
            }
        }

        public void Subscribe<TEvent>(ReceivedMessage received)
            where TEvent : IEvent
        {
            var eventName = typeof(TEvent).Name;

            _logger?.LogInformation($"Executando { nameof(Subscribe) }");

            if (_connection.Disconnected)
            {
                _connection.Connect();
            }

            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: eventName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, args) =>
                {
                    var body = args.Body.ToArray();
                    var message = body.ToText();
                    var @event = message.To<TEvent>();

                    received?.Invoke(new ReceivedMessageEventArgs(@event));

                    _logger?.LogInformation("Mensagem recebida pela fila {QueueName}: {Message}", eventName, message);
                };

                channel.BasicConsume(
                    queue: eventName,
                    autoAck: true,
                    consumer: consumer);
            }
        }

        private void DeclareQueueAndBind(string eventName, IModel channel)
        {
            _logger?.LogInformation("Declarando Queue: '{QueueName}'", eventName);

            channel.QueueDeclare(
                queue: eventName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            channel.QueueBind(
                queue: eventName,
                exchange: _rabbitMQSettings.ExchangeName,
                routingKey: eventName
            );
        }

        private void DeclareExchange(IModel channel)
        {
            _logger?.LogInformation("Declarando Exchange: '{ExchangeName}'", _rabbitMQSettings.ExchangeName);

            channel.ExchangeDeclare(
                exchange: _rabbitMQSettings.ExchangeName,
                type: "direct",
                durable: false,
                autoDelete: false,
                null
            );
        }
    }
}
