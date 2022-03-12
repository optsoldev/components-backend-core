using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class SwaggerSettingsNullException : Exception
    {
        public SwaggerSettingsNullException(ILoggerFactory logger = null)
            : base("A configuração de SEGURANÇA não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(SwaggerSettingsNullException));
            _logger?.LogCritical(
@$"{nameof(SwaggerSettingsNullException)}:
""SwaggerSettings"": {{
    ""Title"": ""{{title}}"",
    ""Name"": ""{{name your swagger}}"",
    ""Enabled"": ""true"",
    ""Version"": ""v1"",
    ""Description"": ""{{your description}}"",
    ""Security"": ""{{
        ""Name"": ""{{OAUTH Name}}"",
        ""Enabled"": ""{{true}}"",
        ""Scopes"": ""{{
            {{<key=name scope>}}:{{<value=description>}}
        }}""
    }}""
  }}
", default);
        }

        protected SwaggerSettingsNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
