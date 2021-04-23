using Microsoft.Extensions.Logging;
using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public sealed class SecuritySettingNullException: Exception
    {
        public SecuritySettingNullException(ILoggerFactory logger)
            : base("A configuração de segurança não foi encontrada no appsettings")
        {
            var _logger = logger?.CreateLogger(nameof(SecuritySettingNullException));
            _logger?.LogCritical(
@$"{nameof(SecuritySettingNullException)}:
""SecuritySettings"": {{
    ""ApiName"": ""name-webapi"",
    ""IsDevelopment"": true|false,
    ""AzureB2C"": {{
        ""Instance"": ""https://domain..."",
        ""ClientId"": ""f008b483-7a32-413d-..."",
        ""Domain"": ""domain.onmicrosoft.com"",
        ""SignedOutCallbackPath"": ""/signout/B2C_1_login"",
        ""SignUpSignInPolicyId"": ""b2c_1_login"",
        ""ResetPasswordPolicyId"": ""b2c_1_reset"",
        ""EditProfilePolicyId"": ""b2c_1_edit"" // Optional profile editing policy
        //""CallbackPath"": ""/signin/B2C_1_sign_up_in""  // defaults to /signin-oidc
    }}
}}
"
            );
        }
    }
}
