﻿using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class CorsSettingsNullException : Exception
    {
        public CorsSettingsNullException(ILoggerFactory logger = null)
            : base("A configuração do CORS não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(CorsSettingsNullException));
            _logger?.LogCritical(
@$"{nameof(CorsSettingsNullException)}:
""CorsSettings"": {{
    ""DefaultPolicy"": ""_corsPolicyDefaultName"",
    ""Policies"": [{{
        ""Name"": ""_corsPolicyName"",
        ""Origins"": {{
            ""FrontHttp"": ""http://domain..."",
            ""FrontHttps"": ""https://domain...""
        }}
    }}]
}}", default
            );
        }

        protected CorsSettingsNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
