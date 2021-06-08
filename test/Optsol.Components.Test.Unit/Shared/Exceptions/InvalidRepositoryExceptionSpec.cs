using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class InvalidRepositoryExceptionSpec
    {
        [Trait("Exceptions", "Exception")]
        [Fact(DisplayName = "Deve inicializar o InvalidRepositoryException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_De_Erro()
        {
            //Given
            InvalidRepositoryException exception;

            //When
            exception = new InvalidRepositoryException();

            //Then
            var msg = "O repositório foi configurado incorretamente";
            exception.Message.Should().Be(msg);
        }
    }
}
