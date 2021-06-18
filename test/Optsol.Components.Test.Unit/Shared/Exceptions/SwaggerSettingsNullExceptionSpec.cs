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
    public class SwaggerSettingsNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o SecuritySettingNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            SwaggerSettingsNullException exception;

            //When
            exception = new SwaggerSettingsNullException(null);

            //Then
            var msg = "A configuração de SEGURANÇA não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);
        }

        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve logar informação referente a falta do settings do SecuritySettingNullException")]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<SecuritySettingNullException>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            SwaggerSettingsNullException exception;

            //When
            exception = new SwaggerSettingsNullException(loggerFactoryMock.Object);

            //Then
            var msg = "A configuração de SEGURANÇA não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Any(a => a.Contains(nameof(SwaggerSettings))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SwaggerSettings.Title))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SwaggerSettings.Name))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SwaggerSettings.Enabled))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SwaggerSettings.Version))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SwaggerSettings.Description))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SwaggerSettings.Security))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SwaggerSettings.Security.Enabled))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SwaggerSettings.Security.Scopes))).Should().BeTrue();
        }
    }
}
