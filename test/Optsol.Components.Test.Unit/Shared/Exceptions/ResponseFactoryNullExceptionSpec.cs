using System;
using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ResponseFactoryNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o ResponseFactoryNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
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
