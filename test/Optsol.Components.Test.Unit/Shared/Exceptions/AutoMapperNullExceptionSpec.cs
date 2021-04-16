using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class AutoMapperNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o AutoMapperNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_De_Erro()
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
