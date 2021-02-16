using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        private readonly IUserService _userService;

        public AuthenticationController(IIdentityServerInteractionService interactionService, IEventService eventService, IUserService userService)
        {
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        [Route("api/sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInApiModel signIn)
        {
            // TODO: error responses
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var validationResult = _userService.Authenticate(signIn.Username, signIn.Password);
            if (!validationResult.IsSuccess)
            {
                await _eventService.RaiseAsync(new UserLoginFailureEvent(signIn.Username, "invalid credentials"));
                return Unauthorized();
            }

            await _eventService.RaiseAsync(new UserLoginSuccessEvent(
                username: signIn.Username,
                subjectId: validationResult.SubjectId,
                name: signIn.Username));

            AuthenticationProperties props = null;

            if (signIn.RememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30)),
                };
            }

            var isuser = new IdentityServerUser(validationResult.SubjectId)
            {
                DisplayName = signIn.Username
            };

            await HttpContext.SignInAsync(isuser.CreatePrincipal(), properties: props);

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
