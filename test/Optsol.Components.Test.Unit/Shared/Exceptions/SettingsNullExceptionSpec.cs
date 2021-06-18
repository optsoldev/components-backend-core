using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class SettingsNullExceptionSpec
    {
        [Trait("Exceptions", "NullException")]
        [Fact(DisplayName = "Deve inicializar o ResponseFactoryNullException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            SettingsNullException exception;

            //When
            exception = new SettingsNullException("Property");

            //Then
            var msg = "O Atributo Property está nulo";
            exception.Message.Should().Be(msg);
        }
    }
}
