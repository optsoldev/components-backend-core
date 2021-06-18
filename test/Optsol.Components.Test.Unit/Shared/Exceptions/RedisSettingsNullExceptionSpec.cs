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
    public class RedisSettingsNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o RedisSettingsNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            ILoggerFactory logger = null;

            //When
            var exception = new RedisSettingsNullException(logger);

            //Then
            var msg = "A configuração do REDIS não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);
        }

        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve logar informação referente a falta do settings do RedisSettingsNullException")]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<RedisSettingsNullException>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);


            //When
            var exception = new RedisSettingsNullException(loggerFactoryMock.Object);

            //Then
            var msg = "A configuração do REDIS não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Any(a => a.Contains(nameof(RedisSettings))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(RedisSettings.ConnectionString))).Should().BeTrue();
        }
    }
}
