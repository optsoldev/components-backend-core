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
    public class CorsSettingsNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o CorsSettingsNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            ILoggerFactory logger = null;

            //When
            var exception = new CorsSettingsNullException(logger);

            //Then
            var msg = "A configuração do CORS não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);
        }

        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve logar informação referente a falta do settings do CorsSettingsNullException")]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<CorsSettingsNullException>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);


            //When
            var exception = new CorsSettingsNullException(loggerFactoryMock.Object);

            //Then
            var msg = "A configuração do CORS não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Any(a => a.Contains(nameof(CorsSettings))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(CorsSettings.DefaultPolicy))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(CorsSettings.Policies))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(CorsPolicy.Name))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(CorsPolicy.Origins))).Should().BeTrue();
        }
    }
}
