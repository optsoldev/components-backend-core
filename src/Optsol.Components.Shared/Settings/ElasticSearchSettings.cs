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
            if (Uri.IsEmpty())
            {
                ShowingException(nameof(Uri));
            }

            if (IndexName.IsEmpty())
            {
                ShowingException(nameof(IndexName));
            }
        }
    }

}
