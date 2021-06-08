using System;
using System.Collections.Generic;

namespace Optsol.Components.Shared.Settings
{
    public class SwaggerSettings : BaseSettings
    {
        public string Title { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public SwaggerSecurity Security { get; set; }

        public override void Validate()
        {
            if (Title.IsEmpty())
            {
                ShowingException(nameof(Title));
            }

            if (Version.IsEmpty())
            {
                ShowingException(nameof(Version));
            }

            if (Name.IsEmpty())
            {
                ShowingException(nameof(Name));
            }
        }
    }

    public class SwaggerSecurity : BaseSettings
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
        
        public bool Enabled { get; set; }

        public IDictionary<string, string> Scopes { get; set; } = new Dictionary<string, string>();

        public override void Validate()
        {
            if (Name.IsEmpty())
            {
                ShowingException(nameof(Name));
            }

            if (ClientId.IsEmpty())
            {
                ShowingException(nameof(ClientId));
            }
        }
    }
}
