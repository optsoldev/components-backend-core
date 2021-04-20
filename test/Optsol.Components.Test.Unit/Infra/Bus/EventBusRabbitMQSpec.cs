using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.Components.Infra.Bus.Delegates;
using Optsol.Components.Infra.Bus.Events;
using Optsol.Components.Infra.RabbitMQ.Connections;
using Optsol.Components.Infra.RabbitMQ.Services;
using Optsol.Components.Shared.Settings;
using Optsol.Components.Test.Shared.Logger;
using System;
using Xunit;

namespace Optsol.Components.Test.Unit.Infra.Bus
{
    public class EventBusRabbitMQSpec
    {
        [Trait("Bus", "RabbitMQ")]
        [Fact(DisplayName = "Deve Testar", Skip = "Teste")]
        public void DeveTestar()
        {
            //Given
            var settings = new RabbitMQSettings
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                ExchangeName = "playgroung_event_bus"
            };

            var logger = new XunitLogger<EventBusRabbitMQ>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            var rabbitMQConnectionMock = new Mock<RabbitMQConnection>(loggerFactoryMock.Object, settings);

            var eventBusRabbitMQ = new EventBusRabbitMQ(settings, loggerFactoryMock.Object, rabbitMQConnectionMock.Object);
            var teste = new Teste
            {
                Nome = Guid.NewGuid().ToString()
            };

            //When
            eventBusRabbitMQ.Subscribe<Teste>(received: EventBusRabbitMQ_OnReceivedMessage);

            eventBusRabbitMQ.Publish(teste);

            //Then
            logger.Logs.Should().NotBeEmpty();



        }

        private void EventBusRabbitMQ_OnReceivedMessage(ReceivedMessageEventArgs e)
        {
            e.Message.Should().NotBeNull();
        }
    }

    public class Teste : IEvent
    {
        public string Nome { get; set; }
    }
}
