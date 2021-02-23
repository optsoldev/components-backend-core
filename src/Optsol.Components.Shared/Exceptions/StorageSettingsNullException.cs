using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class StorageSettingsNullException : Exception
    {

        public StorageSettingsNullException(ILogger<StorageSettingsNullException> logger = null)
            : base("A configuração do Storage não foi encontrada no appsettings")
        {
            logger?.LogCritical(
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
