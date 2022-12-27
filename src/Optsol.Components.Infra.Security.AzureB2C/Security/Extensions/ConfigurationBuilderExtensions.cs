using Microsoft.Extensions.Configuration;
using Optsol.Components.Shared.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationBuilderExtensions
{
    public static void AddInMemoryObject(this ConfigurationBuilder configurationBuilder, object settings, string settingsRoot)
    {
        configurationBuilder.AddInMemoryCollection(settings.ToKeyValuePairs(settingsRoot));
    }       
}