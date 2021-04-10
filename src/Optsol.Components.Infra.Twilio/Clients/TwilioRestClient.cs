using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using Twilio;
using Twilio.Clients;

namespace Optsol.Components.Infra.Twilio.Clients
{
    public class TwilioRestClient
    {
        protected readonly TwilioSettings _twilioSettings;

        public TwilioRestClient(TwilioSettings twilioSettings)
        {
            _twilioSettings = twilioSettings ?? throw new TwilioSettingsNullException();

            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
        }

        public ITwilioRestClient GetClient()
        {
            return TwilioClient.GetRestClient();
        }
    }
}
