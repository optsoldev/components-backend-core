using System;

namespace Optsol.Components.Shared.Settings
{
    public class ApplicationInsightsSettings : BaseSettings
    {
        public string InstrumentationKey { get; set; }

        public override void Validate()
        {
            if (InstrumentationKey.IsEmpty())
            {
                ShowingException(nameof(InstrumentationKey));
            }
        }
    }
}
