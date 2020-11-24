using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ConnectionStringNullExceptionSpec
    {
        [Fact]
        public void DeveInicializarComMensagemDeErro()
        {
            //Given
            ConnectionStringNullException exception;;  

            //When
            exception = new ConnectionStringNullException();

            //Then
            var msg = "A string de conexão não foi encontrada";
            exception.Message.Should().Be(msg);
        }
    }
}
