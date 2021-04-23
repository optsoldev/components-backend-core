using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;
using System.Linq;

namespace Optsol.Components.Infra.Security.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OptsolAuthorizeAttribute : TypeFilterAttribute
    {
        public OptsolAuthorizeAttribute(string claim) : base(typeof(OptsolAuthorizeFilterAttribute))
        {
            Arguments = new object[] { claim };

        }
    }

    public class OptsolAuthorizeFilterAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _claim;

        private readonly SecuritySettings _securitySettings;

        public OptsolAuthorizeFilterAttribute(SecuritySettings securitySettings, ILoggerFactory logger, string claim)
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

        private static void ContextRemoteSecurity(AuthorizationFilterContext context)
        {
            //TODO: Validar com o SSO.

            context.Result = new UnauthorizedResult();
        }
    }
}
