namespace Optsol.Components.Infra.Security.AzureB2C.Security.Models
{
    public class OauthClient
    {
        public string? Instance { get; set; }

        public string? ClientId { get; set; }

        public string? Domain { get; set; }

        public string? SignedOutCallbackPath { get; set; }

        public string? SignUpSignInPolicyId { get; set; }

        public string? ResetPasswordPolicyId { get; set; }

        public string? EditProfilePolicyId { get; set; }
    }
}
