using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Security.Constants;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;
using System.Linq;

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

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SecuritySettings _securitySettings;

        public OptsolAuthorizeFilter(UserManager<ApplicationUser> userManager,
            SecuritySettings securitySettings,
            ILogger<SecuritySettingNullException> logger,
            string claim)
        {
            _claim = claim;

            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

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

            if(userAuthenticateHasClaim)
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }

        private void ContextRemoteSecurity(AuthorizationFilterContext context)
        {
            var contextUser = context.HttpContext.User;
            var nameUserForFilterNormalizedName = contextUser.FindFirst(SecurityClaimTypes.UserName).Value;

            var applicationUser = _userManager.Users
                .Include(x => x.Claims)
                .SingleAsync(x => x.NormalizedUserName == nameUserForFilterNormalizedName)
                .GetAwaiter()
                .GetResult();

            var userAuthenticateHasClaim = contextUser.Identity.IsAuthenticated
                && applicationUser.Claims.Any(c => c.ClaimType.Equals("optsol")
                && c.ClaimValue.Equals(_claim, StringComparison.OrdinalIgnoreCase));

            if (userAuthenticateHasClaim)
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
