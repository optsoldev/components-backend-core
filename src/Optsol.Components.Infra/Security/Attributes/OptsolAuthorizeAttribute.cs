using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Security.Services;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

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
        private readonly IAuthorityService _authorityService;

        public OptsolAuthorizeFilterAttribute(SecuritySettings securitySettings, IAuthorityService authorityService, ILoggerFactory logger, params string[] claims)
        {
            _claim = claims;

            _securitySettings = securitySettings ?? throw new SecuritySettingNullException(logger);
            _authorityService = authorityService ?? throw new ArgumentNullException(nameof(authorityService));

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
            var accessToken = context.HttpContext.Request.Headers["Authorization"];
            var parse = AuthenticationHeaderValue.Parse(accessToken);

            var response = _authorityService.GetUserInfo(parse.Parameter)
                .GetAwaiter()
                .GetResult();

            var userAuthenticateHasClaim = _claim.All(claim => response.HasAccess(claim));
            if (userAuthenticateHasClaim)
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
