using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class AsyncEnumerableNullExceptionSpec
    {
        [Fact]
        public void DeveInicializarComMensagemDeErro()
        {
            //Given
            AsyncEnumerableNullException exception;;  

            //When
            exception = new AsyncEnumerableNullException();

            //Then
            var msg = "O argumento IAsyncEnumerable está nulo";
            exception.Message.Should().Be(msg);
        }
    }
}
