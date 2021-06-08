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
    public class SecuritySettingsNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o SecuritySettingNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            SecuritySettingNullException exception;

            //When
            exception = new SecuritySettingNullException(null);

            //Then
            var msg = "A configuração de segurança não foi encontrada no appsettings";
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

            SecuritySettingNullException exception;

            //When
            exception = new SecuritySettingNullException(loggerFactoryMock.Object);

            //Then
            var msg = "A configuração de segurança não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Any(a => a.Contains(nameof(SecuritySettings))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SecuritySettings.ApiName))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SecuritySettings.Development))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SecuritySettings.Authority))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SecuritySettings.Authority.ClientId))).Should().BeTrue();
            logger.Logs.Any(a => a.Contains(nameof(SecuritySettings.Authority.Url))).Should().BeTrue();
        }
    }
}