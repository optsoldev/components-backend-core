using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class StorageSettingsNullException : Exception
    {

        public StorageSettingsNullException(ILoggerFactory logger)
            : base("A configuração do STORAGE não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(StorageSettingsNullException));
            _logger?.LogCritical(
@$"{nameof(StorageSettingsNullException)}:
""StorageSettings"": {{
    {{
        ""ConnectionString"": ""{{UseDevelopmentStorage=true}}""
    }}
}}
", default
            );
        }

        protected StorageSettingsNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
