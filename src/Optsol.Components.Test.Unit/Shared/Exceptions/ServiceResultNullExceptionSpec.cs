using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class ServiceResultNullExceptionSpec
    {
        [Fact]
        public void DeveInicializarComMensagemDeErro()
        {
            //Given
            ServiceResultNullException exception;;  

            //When
            exception = new ServiceResultNullException();

            //Then
            var msg = "O parametro IServiceResult não foi resolvido pela IoC";
            exception.Message.Should().Be(msg);
        }
    }
}
