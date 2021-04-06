using System;

namespace Optsol.Components.Shared.Settings
{
    public class ElasticSearchSettings : BaseSettings
    {
        public string IndexName { get; set; }
        public string Uri { get; set; }

        public override void Validate()
        {
            var uriIsNullOrEmpty = string.IsNullOrEmpty(Uri);
            if (uriIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(Uri));
            }
        }
    }

}
