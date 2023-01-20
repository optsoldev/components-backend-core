using System.ComponentModel;
using FluentAssertions;
using Optsol.Components.Shared.Extensions;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Extensions;

public class EnumExtensionsSpec
{
    [Trait("Extensions", "EnumExtensions")]
    [Fact(DisplayName = "Deve Buscar String")]
    public void DeveBuscarDescriptionDoEnum()
    {
        var teste1 = TestEnum.Teste1;

        var description = teste1.GetDescription();

        description.Should().Be("Teste Um");
    }

    public enum TestEnum
    {
        [Description("Teste Um")]
        Teste1 = 1,
        [Description("Teste Dois")]
        Teste2 = 2
    }
}