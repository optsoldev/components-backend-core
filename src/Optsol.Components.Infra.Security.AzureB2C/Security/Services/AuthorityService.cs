using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Security.AzureB2C.Security.Models;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Infra.Security.AzureB2C.Security.Services
{
    public class AuthorityService : IAuthorityService
    {
        private readonly ILogger<AuthorityService> _logger;

        public AuthorityService(ILogger<AuthorityService> logger)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando AuthorityService");
        }

        public Task<OauthClient> GetClient(SecuritySettings securitySettings)
        {
            return Task.FromResult(new OauthClient
            {
                Instance = securitySettings.Authority.Instance,
                ClientId = securitySettings.Authority.ClientId,
                Domain = securitySettings.Authority.Domain,
                SignedOutCallbackPath = "/signout/B2C_1_login",
                SignUpSignInPolicyId = "b2c_1_login",
                ResetPasswordPolicyId = "b2c_1_reset",
                EditProfilePolicyId = "b2c_1_edit",
            });
        }
    }
}
