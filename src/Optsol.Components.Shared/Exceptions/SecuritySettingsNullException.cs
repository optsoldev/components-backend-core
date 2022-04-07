using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class SecuritySettingNullException: Exception
    {
        public SecuritySettingNullException(ILoggerFactory logger)
            : base("A configuração de segurança não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(SecuritySettingNullException));
            _logger?.LogCritical(
@$"{nameof(SecuritySettingNullException)}:
""SecuritySettings"": {{
    ""ApiName"": ""name-webapi"",
    ""Development"": true|false,
    ""Authority"": {{
        ""ClientId"": ""f008b483-7a32-413d-..."",
        ""Endpoint"": ""http://ssodomain""
    }}
}}
", default
            );
        }

        protected SecuritySettingNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
