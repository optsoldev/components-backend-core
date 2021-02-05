using System;

namespace Optsol.Components.Shared.Settings
{
    public class SecuritySettings : BaseSettings
    {
        public string IdentityConnection { get; set; }

        public override void Validate()
        {
            var identityConnectionIsNullOrEmpty = string.IsNullOrEmpty(IdentityConnection);
            if (identityConnectionIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(IdentityConnection));
            }
        }
    }
}
