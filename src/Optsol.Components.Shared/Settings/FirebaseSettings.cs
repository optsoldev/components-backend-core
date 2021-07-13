using Optsol.Components.Shared.Extensions;
using System;

namespace Optsol.Components.Shared.Settings
{
    public class FirebaseSettings : BaseSettings
    {
        public string Url { get; set; }

        public string Key { get; set; }

        public override void Validate()
        {
            if (Url.IsValidEndpoint().Not())
            {
                ShowingException(nameof(Url));
            }

            if (Key.IsEmpty())
            {
                ShowingException(nameof(Key));
            }
        }
    }
}
