using FluentAssertions;
using Optsol.Components.Domain.Entities;
using System;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared
{
    public class GenericExtensionsSpec
    {
        [Trait("Extensions", "GenericExtensions")]
        [Fact(DisplayName = "Deve Converter um objeto em Json")]
        public void Deve_Converter_Objeto_Json()
        {
            //Given
            var aggregateRoot = new AggregateRoot();

            //When
            var aggregateRootJson = aggregateRoot.ToJson();

            //Then
            aggregateRootJson.StartsWith("{").Should().BeTrue();
            aggregateRootJson.EndsWith("}").Should().BeTrue();
            aggregateRootJson.Contains(":").Should().BeTrue();
            aggregateRootJson.Contains(aggregateRoot.ToString()).Should().BeTrue();
            aggregateRootJson.Contains(nameof(aggregateRoot.CreatedDate)).Should().BeTrue();
        }
    }
}
