using System;

namespace Optsol.Components.Shared.Settings
{
    public class ElasticSearchSettings : BaseSettings
    {
        public string Uri { get; set; }

        public string IndexName { get; set; }
        
        public string UserName { get; set; }

        public string Password { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Uri))
            {
                throw new ArgumentNullException(nameof(Uri));
            }

            if (string.IsNullOrEmpty(IndexName))
            {
                throw new ArgumentNullException(nameof(IndexName));
            }
        }
    }

}
