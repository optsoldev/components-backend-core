using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace Optsol.Components.Infra.Security.AzureB2C.Security.Policies
{
    public class ScopesRequirement : AuthorizationHandler<ScopesRequirement>, IAuthorizationRequirement
    {
        private readonly string[] _acceptedScopes;

        public ScopesRequirement(params string[] acceptedScopes)
        {
            _acceptedScopes = acceptedScopes;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopesRequirement requirement)
        {
            if (!context.User.Claims.Any(x => x.Type == ClaimConstants.Scope)
               && !context.User.Claims.Any(y => y.Type == ClaimConstants.Scp))
            {
                return Task.CompletedTask;
            }

            var scopeClaim = context.User.FindFirst(ClaimConstants.Scp);

            if (scopeClaim == null)
                scopeClaim = context.User.FindFirst(ClaimConstants.Scope);

            if (scopeClaim != null && scopeClaim.Value.Split(' ').Intersect(requirement._acceptedScopes).Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
