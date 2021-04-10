using System;

namespace Optsol.Components.Shared.Settings
{
    public class TwilioSettings : BaseSettings
    {
        public string AccountSid { get; set; }

        public string AuthToken { get; set; }

        public override void Validate()
        {
            var accountSidIsNullOrEmpty = string.IsNullOrEmpty(AccountSid);
            if (accountSidIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(AccountSid));
            }

            var authTokenIsNullOrEmpty = string.IsNullOrEmpty(AuthToken);
            if (authTokenIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(AuthToken));
            }
        }
    }
}
