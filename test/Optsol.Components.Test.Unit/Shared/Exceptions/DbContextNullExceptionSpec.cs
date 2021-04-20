using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class DbContextNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o DbContextNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            DbContextNullException exception;;  

            //When
            exception = new DbContextNullException();

            //Then
            var msg = "O parametro DBContext não foi resolvido pela injeção de dependência";
            exception.Message.Should().Be(msg);
        }
    }
}
