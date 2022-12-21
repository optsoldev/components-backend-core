using System;
using System.Collections.Generic;

namespace Optsol.Components.Shared.Settings
{
    public class CorsSettings : BaseSettings
    {
        public CorsPolicy DefaultPolicy { get; set; }
        public IEnumerable<CorsPolicy> Policies { get; set; }

        public override void Validate()
        {
            DefaultPolicy.Validate();
        }
    }

    public class CorsPolicy : BaseSettings
    {
        public string Name { get; set; }

        public string[] Origins { get; set; } = Array.Empty<string>();

        public override void Validate()
        {
            if (Name.IsEmpty())
            {
                ShowingException(nameof(Name));
            }
        }
    }
}
