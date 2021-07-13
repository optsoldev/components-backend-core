using Newtonsoft.Json;

namespace Optsol.Components.Infra.Firebase.Models.Response
{
    public class CloudMessagingMessageResponse
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}
