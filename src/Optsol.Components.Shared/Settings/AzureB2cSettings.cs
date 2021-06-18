using System;

namespace Optsol.Components.Shared.Settings
{
    public class AzureB2cSettings : BaseSettings
    {
        public string Instance { get; set; }

        public string ClientId { get; set; }

        public string Domain { get; set; }

        public string SignedOutCallbackPath { get; set; }

        public string SignUpSignInPolicyId { get; set; }

        public string ResetPasswordPolicyId { get; set; }

        public string EditProfilePolicyId { get; set; }

        public string CallbackPath { get; set; }

        public override void Validate()
        {
            if (Instance.IsEmpty())
            {
                ShowingException(nameof(Instance));
            }

            if (ClientId.IsEmpty())
            {
                ShowingException(nameof(ClientId));
            }

            if (Domain.IsEmpty())
            {
                ShowingException(nameof(Domain));
            }

            if (SignedOutCallbackPath.IsEmpty())
            {
                ShowingException(nameof(SignedOutCallbackPath));
            }

            if (SignUpSignInPolicyId.IsEmpty())
            {
                ShowingException(nameof(SignUpSignInPolicyId));
            }

            if (ResetPasswordPolicyId.IsEmpty())
            {
                ShowingException(nameof(ResetPasswordPolicyId));
            }

            if (EditProfilePolicyId.IsEmpty())
            {
                ShowingException(nameof(EditProfilePolicyId));
            }
        }
    }
}
