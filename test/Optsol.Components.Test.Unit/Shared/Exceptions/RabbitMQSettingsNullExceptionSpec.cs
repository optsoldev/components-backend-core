using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using Optsol.Components.Test.Shared.Logger;
using System.Linq;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class RabbitMQSettingsNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o RabbitMQSettingsNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            ILoggerFactory logger = null;

            //When
            var exception = new RabbitMQSettingsNullException(logger);

            //Then
            var msg = "A configuração do RabbitMQ não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);
        }

        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve logar informação referente a falta do settings do RabbitMQSettingsNullException")]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<RabbitMQSettingsNullException>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);


            //When
            var exception = new RabbitMQSettingsNullException(loggerFactoryMock.Object);

            //Then
            var msg = "A configuração do RabbitMQ não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Any(a => a.Contains(nameof(RabbitMQSettings))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(RabbitMQSettings.HostName))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(RabbitMQSettings.ExchangeName))).Should().BeTrue();
        }
    }
}
