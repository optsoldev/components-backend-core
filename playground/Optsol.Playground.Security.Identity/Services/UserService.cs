using Microsoft.AspNetCore.Identity;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Infra.Security.Response;
using Optsol.Components.Infra.Security.Services;
using System.Threading.Tasks;

namespace Optsol.Playground.Security.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ValidationResult> Authenticate(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username).ConfigureAwait(false);
            if (user == null)
            {
                return ValidationResult.Failure();
            }

            var validatePassword = await _signInManager.UserManager.CheckPasswordAsync(user, password);
            return validatePassword
                ? ValidationResult.Success(user.Id.ToString())
                : ValidationResult.Failure();
        }
    }
}
