using FluentAssertions;
using Optsol.Components.Shared.Extensions;
using System;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Extensions
{
    public class RegexExtensionsSpec
    {

        [Trait("Extensions", "RegexExtensions")]
        [Theory(DisplayName = "Deve testar url por regex")]
        [InlineData("example:8080", "http://example:8080", "https://example.com")]
        public void Deve_Testar_Url(string url1, string url2, string url3)
        {
            url1.IsUrlValid().Should().BeFalse();
            url2.IsUrlValid().Should().BeTrue();
            url3.IsUrlValid().Should().BeTrue();
        }
    }
}
