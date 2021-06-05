using Optsol.Components.Shared.Extensions;
using System;
using System.Text.RegularExpressions;

namespace Optsol.Components.Shared.Settings
{
    public class SecuritySettings : BaseSettings
    {
        public bool Development { get; set; }

        public string ApiName { get; set; }

        public Authority Authority { get; set; }

        public override void Validate()
        {
            if (Development)
            {
                return;
            }

            var apiNameIsNullOrEmpty = string.IsNullOrEmpty(ApiName);
            if (apiNameIsNullOrEmpty)
            {
                throw new ApplicationException(nameof(ApiName));
            }


        }
    }

    public class Authority : BaseSettings
    {
        public string Url { get; set; }

        public string ClientId { get; set; }

        public override void Validate()
        {
            var invalidUrl = Url.EndsWith("/");
            if (invalidUrl)
            {
                throw new ApplicationException(nameof(Url));
            }

            var invalidClient = !Url.IsUrlValid();
            if (invalidClient)
            {
                throw new ApplicationException(nameof(Url));
            }
        }
    }
}
