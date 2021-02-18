using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Optsol.Components.Infra.Security.Constants;
using Optsol.Components.Infra.Security.Data;
using System.Collections.Generic;
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
            var user = await _userManager.GetUserAsync(context.Subject);

            //var claims = user.Claims.Select(s => new Claim(s.ClaimType, s.ClaimValue));

            var claims = new List<Claim>
            {
                new Claim(SecurityClaimTypes.UserName, user.UserName)
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null) && user.IsEnabled;
        }
    }
}
