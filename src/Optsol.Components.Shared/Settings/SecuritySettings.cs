using System;

namespace Optsol.Components.Shared.Settings
{
    public class SecuritySettings : BaseSettings
    {
        public string ApiName { get; set; }

        public string Authority { get; set; }

        public bool IsDevelopment { get; set; }

        public override void Validate()
        {
            var apiNameIsNullOrEmpty = !IsDevelopment && string.IsNullOrEmpty(ApiName);
            if (apiNameIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(ApiName));
            }

            var authorityIsNullOrEmpty = !IsDevelopment && string.IsNullOrEmpty(Authority);
            if (authorityIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(Authority));
            }
        }
    }
}
