using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Infra.Security.AzureB2C.Security.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class OptsolAuthorizeAttribute : TypeFilterAttribute
    {
        public OptsolAuthorizeAttribute(params string[] claims) : base(typeof(OptsolAuthorizeFilterAttribute))
        {
            Arguments = new object[] { claims };
        }
    }

    public class OptsolAuthorizeFilterAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] claims;

        private readonly SecuritySettings securitySettings;

        public OptsolAuthorizeFilterAttribute(SecuritySettings securitySettings, ILoggerFactory logger, params string[] claims)
        {
            this.claims = claims;

            this.securitySettings = securitySettings ?? throw new SecuritySettingNullException(logger);
           
            AuthenticationSchemes = "Bearer";
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
            var contextUser = context.HttpContext.User;

            if (!contextUser.IsAuthenticated()) context.Result = new UnauthorizedResult();

            if (claims.Length == 0) return;

            var securityClaim = contextUser.Claims
                .FirstOrDefault(c => c.Type.Equals(securitySettings.SecurityClaim));

            var userClaims = securityClaim?.Value.Split(";");

            var userAuthenticateHasClaim = userClaims is not null && claims.All(claim => userClaims.Contains(claim));
            
            if (userAuthenticateHasClaim)
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
