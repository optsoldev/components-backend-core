using System;
using System.Collections.Generic;

namespace Optsol.Components.Shared.Settings
{
    public class SwaggerSettings : BaseSettings
    {
        public string Title { get; set; }

        public bool Enabled { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public SwaggerSecurity Security { get; set; }

        public override void Validate()
        {
            var titleIsNullOrEmpty = string.IsNullOrEmpty(Title);
            if (titleIsNullOrEmpty)
            {
                throw new ApplicationException(nameof(Title));
            }

            var versionIsNullOrEmpty = string.IsNullOrEmpty(Version);
            if (versionIsNullOrEmpty)
            {
                throw new ApplicationException(nameof(Version));
            }

            var nameIsNullOrEmpty = string.IsNullOrEmpty(Name);
            if (nameIsNullOrEmpty)
            {
                throw new ApplicationException(nameof(Name));
            }
        }
    }

    public class SwaggerSecurity : BaseSettings
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public IDictionary<string, string> Scopes { get; set; } = new Dictionary<string, string>();

        public override void Validate()
        {
            var nameIsNullOrEmpty = string.IsNullOrEmpty(Name);
            if (nameIsNullOrEmpty)
            {
                throw new ApplicationException(nameof(Name));
            }
        }
    }
}
