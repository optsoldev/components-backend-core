using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Twilio.Clients;
using System;

namespace Optsol.Components.Infra.Twilio.Video
{
    public class TwilioVideo : ITwilioVideo
    {
        protected readonly ILogger<TwilioVideo> _logger;
        protected readonly TwilioRestClient _twilioClient;

        public TwilioVideo(TwilioRestClient twilioRestClient, ILogger<TwilioVideo> logger)
        {
            _logger = logger;
            _twilioClient = twilioRestClient ?? throw new ArgumentNullException(nameof(twilioRestClient));
        }
    }
}
