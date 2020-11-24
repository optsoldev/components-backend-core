using Xunit;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Shared.Extensions;
using FluentAssertions;

namespace Optsol.Components.Test.Unit.Shared
{
    public class JsonExtensionsSpec
    {

        [Fact]
        public void DeveConverterObjetoEmJson()
        {
            //Given
            AggregateRoot aggregateRoot = new AggregateRoot();

            //When
            var aggregateRootJson = aggregateRoot.ToJson();
            
            //Then
            aggregateRootJson.StartsWith("{").Should().BeTrue();
            aggregateRootJson.EndsWith("}").Should().BeTrue();
            aggregateRootJson.Contains(":").Should().BeTrue();
            aggregateRootJson.Contains(aggregateRoot.ToString()).Should().BeTrue();
            aggregateRootJson.Contains(nameof(aggregateRoot.CreateDate)).Should().BeTrue();
        }
    }
}
