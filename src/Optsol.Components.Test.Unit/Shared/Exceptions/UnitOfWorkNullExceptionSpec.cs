using System;
using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Exceptions
{
    public class UnitOfWorkNullExceptionSpec
    {
        [Fact]
        public void DeveInicializarComMensagemDeErro()
        {
            //Given
            UnitOfWorkNullException exception;;  

            //When
            exception = new UnitOfWorkNullException();

            //Then
            var msg = "O parametro unitOfWork não foi resolvido pela IoC";
            exception.Message.Should().Be(msg);
        }
    }
}
