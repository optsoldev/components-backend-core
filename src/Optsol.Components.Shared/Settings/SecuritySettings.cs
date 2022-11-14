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

            Authority.Validate();
        }
    }

    public class Authority : BaseSettings
    {
        public string ClientId { get; set; }

        public string Instance { get; set; }

        public string Domain { get; set; }

        public override void Validate()
        {
            if (Instance.IsValidEndpoint().Not())
            {
                ShowingException(nameof(Instance));
            }

            if (Domain.IsEmpty())
            {
                ShowingException(nameof(Domain));
            }

            if (ClientId.IsEmpty())
            {
                ShowingException(nameof(ClientId));
            }
        }
    }
}
