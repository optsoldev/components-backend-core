using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Test.Shared.Logger;
using System.Linq;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class SecuritySettingsNullExceptionSpec
    {
        [Fact]
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

        [Fact]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<SecuritySettingNullException>();

            SecuritySettingNullException exception;

            //When
            exception = new SecuritySettingNullException(logger);

            //Then
            var msg = "A configuração de segurança não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Any(a => a.Contains("SecuritySettings")).Should().BeTrue();
        }
    }
}