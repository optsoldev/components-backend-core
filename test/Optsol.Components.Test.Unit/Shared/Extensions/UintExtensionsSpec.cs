using FluentAssertions;
using System;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Extensions
{
    public class UintExtensionsSpec
    {
        [Trait("Extensions", "UintExtensions")]
        [Fact(DisplayName = "Deve converter um uint em int")]
        public void Deve_Converter_Uint_Em_Int()
        {
            //Given
            uint @uint = 10;

            //When
            var uintToInt = @uint.ToInt();

            //Then
            uintToInt.Should().BePositive();
        }
    }
}
