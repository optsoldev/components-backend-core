using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class UnitOfWorkNullExceptionSpec
    {
        [Fact]
        public void DeveInicializarComMensagemDeErro()
        {
            //Given
            UnitOfWorkNullException exception;;  

            //When
            exception = new UnitOfWorkNullException();

            //Then
            var msg = "O parametro UnitOfWork não foi resolvido pela injeção de dependência";
            exception.Message.Should().Be(msg);
        }
    }
}
