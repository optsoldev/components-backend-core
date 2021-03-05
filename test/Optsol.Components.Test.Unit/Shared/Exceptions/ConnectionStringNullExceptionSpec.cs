using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Test.Shared.Logger;
using System.Linq;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ConnectionStringNullExceptionSpec
    {
        [Fact]
        public void Deve_Inicializar_Com_Mensagem_Erro()
        {
            //Given
            ILoggerFactory logger = null;

            //When
            var exception = new ConnectionStringNullException(logger);

            //Then
            var msg = "A string de conexão não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);
        }

        [Fact]
        public void Deve_Logar_Informacao_Referente_Falta_Settings()
        {
            //Given
            var logger = new XunitLogger<ConnectionStringNullException>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(setup => setup.CreateLogger(It.IsAny<string>())).Returns(logger);

            //When
            var exception = new ConnectionStringNullException(loggerFactoryMock.Object);

            //Then
            var msg = "A string de conexão não foi encontrada no appsettings";
            exception.Message.Should().Be(msg);

            logger.Logs.Should().NotBeEmpty();
            logger.Logs.Any(a => a.Contains("ConnectionStrings")).Should().BeTrue();
            logger.Logs.Any(a => a.Contains("DefaultConnection")).Should().BeTrue();
            logger.Logs.Any(a => a.Contains("IdentityConnection")).Should().BeTrue();
        }
    }
}
