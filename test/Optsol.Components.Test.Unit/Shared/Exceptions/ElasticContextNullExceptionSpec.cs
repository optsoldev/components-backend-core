using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ElasticContextNullExceptionSpec
    {
        [Fact]
        public void DeveInicializarComMensagemDeErro()
        {
            //Given
            ElasticContextNullException exception;

            //When
            exception = new ElasticContextNullException();

            //Then
            var msg = "O parametro ElasticContext não foi resolvido pela injeção de dependência";
            exception.Message.Should().Be(msg);
        }
    }
}
