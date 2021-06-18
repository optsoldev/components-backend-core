using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class NotificationContextExceptionSpec
    {
        [Trait("Exceptions", "Exception")]
        [Fact(DisplayName = "Deve inicializar o InvalidRepositoryException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_De_Erro()
        {
            //Given
            NotificationContextException exception;

            //When
            exception = new NotificationContextException();

            //Then
            var msg = "O parametro NotificationContext não foi resolvido pela injeção de dependência";
            exception.Message.Should().Be(msg);
        }
    }
}
