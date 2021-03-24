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
        public OptsolAuthorizeAttribute(string claim) : base(typeof(OptsolAuthorizeFilter))
        {
            Arguments = new object[] { claim };

        }
    }

    public class OptsolAuthorizeFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        private string _claim;

        private readonly SecuritySettings _securitySettings;

        public OptsolAuthorizeFilter(SecuritySettings securitySettings, ILoggerFactory logger, string claim)
        {
            _claim = claim;

            _securitySettings = securitySettings ?? throw new SecuritySettingNullException(logger);

            AuthenticationSchemes = "Bearer";
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_securitySettings.IsDevelopment)
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
            var userAuthenticateHasClaim = contextUser.Claims.Any(w => w.Type.Equals("optsol")
                && w.Value.Equals(_claim, StringComparison.OrdinalIgnoreCase));

            if (userAuthenticateHasClaim)
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }

        private void ContextRemoteSecurity(AuthorizationFilterContext context)
        {
            //var accessToken = context.HttpContext.GetTokenAsync("Bearer", "access_token")
            //    .GetAwaiter()
            //    .GetResult();

            //var client = new HttpClient();
            //client.SetBearerToken(accessToken);

            //var response = client
            //    .GetUserInfoAsync(new UserInfoRequest
            //    {
            //        Address = $"{_securitySettings.Authority}/connect/userinfo",
            //        Token = accessToken,
            //    })
            //    .GetAwaiter()
            //    .GetResult();

            //var userAuthenticateHasClaim = response.Claims.Any(c => c.Type.Equals("optsol") && c.Value.Equals(_claim, StringComparison.OrdinalIgnoreCase));
            //if (userAuthenticateHasClaim)
            //{
            //    return;
            //}

            context.Result = new UnauthorizedResult();
        }
    }
}
