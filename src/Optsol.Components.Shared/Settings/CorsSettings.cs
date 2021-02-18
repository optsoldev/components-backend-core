using System;
using System.Collections.Generic;

namespace Optsol.Components.Shared.Settings
{
    public class CorsSettings : BaseSettings
    {
        public string DefaultPolicy { get; set; }

        public IEnumerable<Policy> Policies { get; set; }

        public override void Validate()
        {
            var defaultPolicyIsNullOrEmpty = string.IsNullOrEmpty(DefaultPolicy);
            if (defaultPolicyIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(DefaultPolicy));
            }
        }
    }

    public class Policy : BaseSettings
    {
        public string Name { get; set; }

        public Dictionary<string, string> Origins { get; set; } = new Dictionary<string, string>();

        public override void Validate()
        {
            var nameIsNullOrEmpty = string.IsNullOrEmpty(Name);
            if (nameIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(nameIsNullOrEmpty));
            }
        }
    }
}
