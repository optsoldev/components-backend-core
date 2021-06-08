using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ConfigurationRepositoryExceptionSpec
    {
        [Trait("Exceptions", "Exception")]
        [Fact(DisplayName = "Deve inicializar o ConfigurationRepositoryException com mensagem de erro")]
        public void Deve_Inicializar_Com_Mensagem_De_Erro()
        {
            //Given
            ConfigurationRepositoryException exception;

            //When
            exception = new ConfigurationRepositoryException();

            //Then
            var msg = "As configurações do repositório estão incorretas.";
            exception.Message.Should().Be(msg);
        }
    }
}
