using FluentAssertions;
using System;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Extensions
{
    public class ByteExtensionsSpec
    {
        [Trait("Extensions", "StringExtensions")]
        [Fact(DisplayName = "Deve converter um uma string em bytes")]
        public void Deve_Converter_String_Em_Bytes()
        {
            //Given
            var bytes = new byte[10];
            new Random().NextBytes(bytes);

            //When
            var bytesToString = bytes.ToText();

            //Then
            bytesToString.Should().NotBeEmpty();
            bytesToString.Should().BeOfType<string>();
        }
    }
}
