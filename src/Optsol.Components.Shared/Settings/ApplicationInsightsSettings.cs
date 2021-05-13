using System;

namespace Optsol.Components.Shared.Settings
{
    public class ApplicationInsightsSettings : BaseSettings
    {
        public string InstrumentationKey { get; set; }

        public override void Validate()
        {
            var instrumentationKeyIsNullOrEmpty = string.IsNullOrEmpty(InstrumentationKey);
            if (instrumentationKeyIsNullOrEmpty)
            {
                throw new ApplicationException(nameof(InstrumentationKey));
            }
        }
    }
}
