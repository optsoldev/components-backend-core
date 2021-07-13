using Newtonsoft.Json;

namespace Optsol.Components.Infra.Firebase.Models.Request
{
    public class CloudMessagingRequest<T>
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("notification")]
        public CloudMessagingNotificationRequest Notification { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
