using System;

namespace Optsol.Components.Shared.Settings
{
    public class SecuritySettings : BaseSettings
    {
        public string ApiName { get; set; }

        public bool IsDevelopment { get; set; }

        public AzureB2C AzureB2C { get; set; }

        public override void Validate()
        {
            var apiNameIsNullOrEmpty = !IsDevelopment && string.IsNullOrEmpty(ApiName);
            if (apiNameIsNullOrEmpty)
            {
                throw new ApplicationException(nameof(ApiName));
            }

            var azureB2cIsNull = !IsDevelopment && AzureB2C == null;
            if (azureB2cIsNull)
            {
                throw new ApplicationException(nameof(AzureB2C));
            }
        }
    }

    public class AzureB2C
    {
        public string Instance { get; set; }

        public string ClientId { get; set; }

        public string Domain { get; set; }

        public string SignedOutCallbackPath { get; set; }

        public string SignUpSignInPolicyId { get; set; }

        public string ResetPasswordPolicyId { get; set; }

        public string EditProfilePolicyId { get; set; }
    }
}
