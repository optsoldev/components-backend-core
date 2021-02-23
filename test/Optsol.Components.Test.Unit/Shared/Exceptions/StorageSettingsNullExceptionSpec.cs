using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Test.Shared.Logger;
using System.Linq;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class StorageSettingsNullExceptionSpec
    {
        [Fact]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            StorageSettingsNullException exception; ;

            //When
            exception = new StorageSettingsNullException(null);

            //Then
            var msg = "A configuração do Storage não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);
        }

        [Fact]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<StorageSettingsNullException>();

            StorageSettingsNullException exception;

            //When
            exception = new StorageSettingsNullException(logger);

            //Then
            var msg = "A configuração do Storage não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Any(a => a.Contains("StorageSettings")).Should().BeTrue();
            logger.Logs.Any(a => a.Contains("ConnectionString")).Should().BeTrue();
            logger.Logs.Any(a => a.Contains("Blob")).Should().BeTrue();
        }
    }
}