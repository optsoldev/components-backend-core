using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ConfigurationSecurityDataExceptionSpec
    {
        [Trait("Exceptions", "Exception")]
        [Fact(DisplayName = "Deve inicializar o ConfigurationSecurityDataException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_De_Erro()
        {
            //Given
            ConfigurationSecurityDataException<IAsyncLifetime> exception;

            //When
            exception = new ConfigurationSecurityDataException<IAsyncLifetime>();

            //Then
            var msg = $"O {typeof(IAsyncLifetime).Name} resolvido pela injeção de dependência";
            exception.Message.Should().Be(msg);
        }
    }
}
