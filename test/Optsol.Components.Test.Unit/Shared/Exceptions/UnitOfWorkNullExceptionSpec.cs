using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class UnitOfWorkNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o UnitOfWorkNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
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
