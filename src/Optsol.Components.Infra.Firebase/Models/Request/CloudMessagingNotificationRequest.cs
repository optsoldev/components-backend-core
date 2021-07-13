using Newtonsoft.Json;

namespace Optsol.Components.Infra.Firebase.Models
{
    public class CloudMessagingNotificationRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
