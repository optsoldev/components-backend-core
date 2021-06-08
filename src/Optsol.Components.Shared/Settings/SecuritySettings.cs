using Optsol.Components.Shared.Extensions;
using System;

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
        public string ClientId { get; set; }
        
        public string Url { get; set; }

        public override void Validate()
        {
            if (Url.IsValidEndpoint().Not())
            {
                ShowingException(nameof(Url));
            }

            if (ClientId.IsEmpty())
            {
                ShowingException(nameof(ClientId));
            }
        }
    }
}
