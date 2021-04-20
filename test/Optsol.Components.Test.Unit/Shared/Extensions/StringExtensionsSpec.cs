using FluentAssertions;
using Optsol.Components.Domain.Entities;
using System;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Extensions
{
    public class StringExtensionsSpec
    {
        [Trait("Extensions", "StringExtensions")]
        [Fact(DisplayName = "Deve converter um uma string em um objeto")]
        public void Deve_Converter_String_Em_Objeto()
        {
            //Given
            var aggregateRoot = new AggregateRoot();
            var aggregateRootJson = aggregateRoot.ToJson();

            //When
            var aggregateRootObj = aggregateRootJson.To<AggregateRoot>();

            //Then
            aggregateRootObj.Should().NotBeNull();
            aggregateRootObj.Should().BeOfType<AggregateRoot>();
        }

        [Trait("Extensions", "StringExtensions")]
        [Fact(DisplayName = "Deve converter string em bytes")]
        public void Deve_Converter_String_Em_Bytes()
        {
            //Given
            string convert = "converter";

            //When
            var bytes = convert.ToBytes();

            //Then
            bytes.Should().NotBeNull();
            bytes.Should().BeOfType<byte[]>();

        }
    }
}
