using FluentAssertions;
using Optsol.Sdk.Shared.Exceptions;
using Xunit;

namespace Optsol.Sdk.Test.Unit.Shared.Exceptions
{
    public class DbContextNullExceptionSpec
    {
        [Fact]
        public void DeveInicializarComMensagemDeErro()
        {
            //Given
            DbContextNullException exception;;  

            //When
            exception = new DbContextNullException();

            //Then
            var msg = "O parametro contexto está nulo.";
            exception.Message.Should().Be(msg);
        }
    }
}
