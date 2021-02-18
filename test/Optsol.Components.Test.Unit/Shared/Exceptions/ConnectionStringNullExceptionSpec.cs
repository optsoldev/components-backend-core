using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Test.Shared.Logger;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ConnectionStringNullExceptionSpec
    {
        [Fact]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            ConnectionStringNullException exception;;  

            //When
            exception = new ConnectionStringNullException();

            //Then
            var msg = "A string de conexão não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);
        }

        [Fact]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<ConnectionStringNullException>();

            ConnectionStringNullException exception;

            //When
            exception = new ConnectionStringNullException(logger);

            //Then
            var msg = "A string de conexão não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
        }
    }
}
