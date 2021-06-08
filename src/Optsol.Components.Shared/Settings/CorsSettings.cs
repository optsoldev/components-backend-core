using System;
using System.Collections.Generic;

namespace Optsol.Components.Shared.Settings
{
    public class CorsSettings : BaseSettings
    {
        public string DefaultPolicy { get; set; }

        public IEnumerable<CorsPolicy> Policies { get; set; }

        public override void Validate()
        {
            if (DefaultPolicy.IsEmpty())
            {
                ShowingException(nameof(DefaultPolicy));
            }
        }
    }

    public class CorsPolicy : BaseSettings
    {
        public string Name { get; set; }

        public Dictionary<string, string> Origins { get; set; } = new Dictionary<string, string>();

        public override void Validate()
        {
            if (Name.IsEmpty())
            {
                ShowingException(nameof(Name));
            }
        }
    }
}
