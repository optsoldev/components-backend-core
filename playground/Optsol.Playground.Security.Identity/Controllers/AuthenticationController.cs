using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Infra.Security.Services;
using Optsol.Playground.Security.Identity.Models;
using System;
using System.Threading.Tasks;

namespace Optsol.Playground.Security.Identity.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IIdentityServerInteractionService _interactionService;

        private readonly IEventService _eventService;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationController(IIdentityServerInteractionService interactionService, IEventService eventService,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpPost]
        [Route("api/sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInApiModel signIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var applicationUser = await _userManager.FindByNameAsync(signIn.Username);
            var validationResult = await _signInManager.PasswordSignInAsync(applicationUser, signIn.Password, signIn.RememberLogin, false);
            var passwordValidationReturnFalse = !validationResult.Succeeded;
            if (passwordValidationReturnFalse)
            {
                await _eventService.RaiseAsync(new UserLoginFailureEvent(signIn.Username, "invalid credentials"));
                return Unauthorized();
            }

            await _eventService.RaiseAsync(new UserLoginSuccessEvent(
                username: signIn.Username,
                subjectId: applicationUser.Id.ToString(),
                name: signIn.Username));

            AuthenticationProperties props = null;

            if (signIn.RememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30)),
                    RedirectUri = signIn.ReturnUrl
                };
            }

            var isuser = new IdentityServerUser(applicationUser.Id.ToString())
            {
                DisplayName = signIn.Username
            };

            await HttpContext.SignInAsync(isuser.CreatePrincipal(), properties: props);
            //var res = await HttpContext.AuthenticateAsync();
            //var at = await HttpContext.GetTokenAsync("access_token");

            if (_interactionService.IsValidReturnUrl(signIn.ReturnUrl) || Url.IsLocalUrl(signIn.ReturnUrl))
            {
                return Ok(new
                {
                    uri = signIn.ReturnUrl,
                });
            }

            return Ok(new
            {
                uri = "/",
            });
        }

        [HttpGet]
        [Route("api/sign-out-context")]
        public async Task<IActionResult> GetSignOutContext([FromQuery] string signOutId = null)
        {
            var apiModel = await GetSignOutApiModel(signOutId);
            return Ok(apiModel);
        }

        [HttpGet]
        [Route("api/signed-out-context")]
        public async Task<IActionResult> GetSignedOutContext([FromQuery] string signOutId = null)
        {
            var apiModel = await GetSignedOutApiModel(signOutId);
            return Ok(apiModel);
        }

        [HttpPost]
        [Route("api/sign-out")]
        public async Task<IActionResult> SignOut([FromBody] SignOutResponseApiModel signOut)
        {
            if (User?.Identity?.IsAuthenticated == false)
            {
                return Ok();
            }

            // Delete local authentication cookie
            await HttpContext.SignOutAsync();

            //Response.Cookies.Delete(".AspNetCore.Identity.Application");
            //Response.Cookies.Delete("idserv.external");
            //Response.Cookies.Delete("idserv.session");
            //Response.Cookies.Delete("Identity.External");

            // Raise the logout event
            await _eventService.RaiseAsync(
                new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

            return Ok();
        }

        private async Task<SignOutApiModel> GetSignOutApiModel(string signOutId)
        {
            var context = await _interactionService.GetLogoutContextAsync(signOutId);

            var apiModel = new SignOutApiModel
            {
                SignOutId = signOutId,
                SignOutPrompt = true, // TODO: get from settings
            };

            if (User?.Identity?.IsAuthenticated != true)
            {
                apiModel.SignOutPrompt = false;
                return apiModel;
            }

            if (context?.ShowSignoutPrompt == false)
            {
                apiModel.SignOutPrompt = false;
                return apiModel;
            }

            return apiModel;
        }

        private async Task<SignedOutApiModel> GetSignedOutApiModel(string signOutId)
        {
            var context = await _interactionService.GetLogoutContextAsync(signOutId);

            return new SignedOutApiModel
            {
                SignOutId = signOutId,
                AutomaticRedirectAfterSignOut = false, // TODO: get from settings
                PostLogoutRedirectUri = context?.PostLogoutRedirectUri,
                ClientName = context?.ClientName,
                SignOutIframeUrl = context?.SignOutIFrameUrl,
            };
        }
    }
}
