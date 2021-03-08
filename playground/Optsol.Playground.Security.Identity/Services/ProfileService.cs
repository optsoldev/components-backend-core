using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Optsol.Components.Infra.Security.Constants;
using Optsol.Components.Infra.Security.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Optsol.Playground.Security.Identity.Services
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var calledFromUserInfoEndpoint = context.Caller == IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint;
            if(calledFromUserInfoEndpoint)
            {
                var sub = context.Subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);

                var claims = user.Claims.Select(s => new Claim(s.ClaimType, s.ClaimValue));

                context.IssuedClaims.AddRange(claims);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);

            context.IsActive = user != null && user.IsEnabled;
        }
    }
}
