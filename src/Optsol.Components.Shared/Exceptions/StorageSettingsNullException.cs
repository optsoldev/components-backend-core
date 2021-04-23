using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public sealed class StorageSettingsNullException : Exception
    {

        public StorageSettingsNullException(ILoggerFactory logger)
            : base("A configuração do Storage não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(StorageSettingsNullException));
            _logger?.LogCritical(
@$"{nameof(StorageSettingsNullException)}:
""StorageSettings"": {{
    {{
        ""ConnectionString"": ""{{UseDevelopmentStorage=true}}""
        Blob: {{
            ""ContainerName"": ""{{nameOfContainer}}"",
        }}
    }}
}}
"
            );
        }
    }
}
