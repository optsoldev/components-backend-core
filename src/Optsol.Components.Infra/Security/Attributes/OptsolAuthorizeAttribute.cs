using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;
using System.Linq;
using System.Net.Http;

namespace Optsol.Components.Infra.Security.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OptsolAuthorizeAttribute : TypeFilterAttribute
    {
        public OptsolAuthorizeAttribute(params string[] claims) : base(typeof(OptsolAuthorizeFilterAttribute))
        {
            Arguments = new object[] { claims };

        }
    }

    public class OptsolAuthorizeFilterAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _claim;

        private readonly SecuritySettings _securitySettings;

        public OptsolAuthorizeFilterAttribute(SecuritySettings securitySettings, ILoggerFactory logger, params string[] claims)
        {
            _claim = claims;

            _securitySettings = securitySettings ?? throw new SecuritySettingNullException(logger);

            AuthenticationSchemes = "Bearer";
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_securitySettings.Development)
            {
                ContextLocalSecurity(context);
            }
            else
            {
                ContextRemoteSecurity(context);
            }
        }

        private void ContextLocalSecurity(AuthorizationFilterContext context)
        {
            var contextUser = context.HttpContext.User;
            var userAuthenticateHasClaim = contextUser.Claims
                .Any(w => w.Type.Equals("optsol")
                    && _claim.Contains(w.Value));

            if (userAuthenticateHasClaim)
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }

        private void ContextRemoteSecurity(AuthorizationFilterContext context)
        {
            var accessToken = context.HttpContext.GetTokenAsync("Bearer", "access_token")
                .GetAwaiter()
                .GetResult();

            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            var response = client
                .GetUserInfoAsync(new UserInfoRequest
                {
                    Address = $"{_securitySettings.Authority}/api/usuario/token/has-accesses",
                    Token = accessToken,
                })
                .GetAwaiter()
                .GetResult();

            // && c.Value.Equals(_claim, StringComparison.OrdinalIgnoreCase)
            var userAuthenticateHasClaim = response.Claims.Any(c => c.Type.Equals("optsol"));
            if (userAuthenticateHasClaim)
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
