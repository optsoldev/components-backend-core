using Optsol.Components.Infra.Twilio.Clients;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Optsol.Components.Infra.Twilio.Common
{
    public class BaseTwilio
    {
        protected readonly TwilioRestClient _twilioRestClient;

        public BaseTwilio(TwilioRestClient twilioRestClient)
        {
            _twilioRestClient = twilioRestClient;
        }

        protected Task<MessageResource> SendSms(string from, string to, string body)
        {
            return Send
                (
                    from: $"+{from}",
                    to: $"+{to}",
                    body: body
                );
        }

        protected Task<MessageResource> SendWhats(string from, string to, string body)
        {
            return Send
                (
                    from: $"whatsapp:+{from}",
                    to: $"whatsapp:+{to}",
                    body: body
                );
        }

        private Task<MessageResource> Send(string from, string to, string body)
        {
            return MessageResource.CreateAsync
                (
                    from: new PhoneNumber(from),
                    to: new PhoneNumber(to),
                    body: body,
                    client: _twilioRestClient.GetClient()
                );
        }
    }
}
