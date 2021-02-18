using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Test.Shared.Logger;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class CorsSettingsNullExceptionSpec
    {
        [Fact]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            CorsSettingsNullException exception;;  

            //When
            exception = new CorsSettingsNullException(null);

            //Then
            var msg = "A string de conexão não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);
        }

        [Fact]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<CorsSettingsNullException>();

            CorsSettingsNullException exception;

            //When
            exception = new CorsSettingsNullException(logger);

            //Then
            var msg = "A string de conexão não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
        }
    }
}
