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
    public class StorageSettingsNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o StorageSettingsNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            ILoggerFactory logger = null;

            //When
            var exception = new StorageSettingsNullException(logger);

            //Then
            var msg = "A configuração do Storage não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);
        }

        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve logar informação referente a falta do settings do StorageSettingsNullException")]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<StorageSettingsNullException>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            //When
            var exception = new StorageSettingsNullException(loggerFactoryMock.Object);

            //Then
            var msg = "A configuração do Storage não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Any(a => a.Contains(nameof(StorageSettings.ConnectionString))).Should().BeTrue();
        }
    }
}