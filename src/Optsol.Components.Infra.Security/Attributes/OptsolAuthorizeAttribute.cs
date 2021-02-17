using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;

namespace Optsol.Components.Infra.Security.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OptsolAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private string _claim;

        public OptsolAuthorizeAttribute(string claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            var userAuthenticateHasClaim = user.Identity.IsAuthenticated && user.HasClaim(c => c.Type.Equals("optsol") && c.Value.Equals(_claim, StringComparison.OrdinalIgnoreCase));
            if (userAuthenticateHasClaim)
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
