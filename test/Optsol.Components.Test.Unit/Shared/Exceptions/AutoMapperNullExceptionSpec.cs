using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class AutoMapperNullExceptionSpec
    {
        [Fact]
        public void DeveInicializarComMensagemDeErro()
        {
            //Given
            AutoMapperNullException exception;;  

            //When
            exception = new AutoMapperNullException();

            //Then
            var msg = "O parametro mapper não foi resolvido pela injeção de dependência";
            exception.Message.Should().Be(msg);
        }
    }
}
