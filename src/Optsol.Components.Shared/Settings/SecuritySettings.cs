using System;

namespace Optsol.Components.Shared.Settings
{
    public class SecuritySettings : BaseSettings
    {
        public string ApiName { get; set; }

        public string Authority { get; set; }

        public override void Validate()
        {
            var apiNameIsNullOrEmpty = string.IsNullOrEmpty(ApiName);
            if (apiNameIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(ApiName));
            }

            var authorityIsNullOrEmpty = string.IsNullOrEmpty(Authority);
            if (authorityIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(Authority));
            }
        }
    }
}
