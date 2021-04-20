using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class MongoContextNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o MongoContextNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            MongoContextNullException exception;

            //When
            exception = new MongoContextNullException();

            //Then
            var msg = "O parametro MongoContext não foi resolvido pela injeção de dependência";
            exception.Message.Should().Be(msg);
        }
    }
}
