using System;

namespace Optsol.Components.Shared.Settings
{
    public class SwaggerSettings
    {
        public string Title { get; set; }

        public bool Enabled { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public void Validate()
        {
            var titleIsNullOrEmpty = string.IsNullOrEmpty(Title);
            if (titleIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(Title));
            }

            var versionIsNullOrEmpty = string.IsNullOrEmpty(Version);
            if (versionIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(Version));
            }

            var nameIsNullOrEmpty = string.IsNullOrEmpty(Name);
            if (nameIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(Name));
            }
        }
    }
}
