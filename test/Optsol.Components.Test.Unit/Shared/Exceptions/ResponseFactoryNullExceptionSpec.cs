using System;
using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ResponseFactoryNullExceptionSpec
    {
        [Fact]
        public void DeveInicializarComMensagemDeErro()
        {
            //Given
            ResponseFactoryNullException exception;;  

            //When
            exception = new ResponseFactoryNullException();

            //Then
            var msg = "O parametro IResponseFactory não foi resolvido pela injeção de dependência";
            exception.Message.Should().Be(msg);
        }
    }
}
