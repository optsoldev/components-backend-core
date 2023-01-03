#nullable enable
using System;
using System.Collections.Generic;

namespace Optsol.Components.Shared.Settings
{
    public class CorsSettings : BaseSettings
    {
        public CorsPolicy? DefaultPolicy { get; set; }
        public IEnumerable<CorsPolicy>? Policies { get; set; }

        public override void Validate()
        {
            DefaultPolicy?.Validate();

            if (Policies is null) return;
            
            foreach (var policy in Policies)
            {
                policy.Validate();
            }
        }
    }

    public class CorsPolicy : BaseSettings
    {
        public string? Name { get; set; }

        public string[] Origins { get; init; } = Array.Empty<string>();

        public override void Validate()
        {
            if (Name.IsEmpty())
            {
                ShowingException(nameof(Name));
            }
        }
    }
}
