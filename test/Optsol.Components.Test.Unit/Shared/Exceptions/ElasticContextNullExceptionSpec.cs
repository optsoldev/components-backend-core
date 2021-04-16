using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ElasticContextNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o ElasticContextNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
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
