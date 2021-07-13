using Newtonsoft.Json;
using System.Collections.Generic;

namespace Optsol.Components.Infra.Firebase.Models.Response
{
    public class CloudMessagingResponse
    {
        [JsonProperty("multicast_id")]
        public string MulticastId { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("failure")]
        public bool Failure { get; set; }

        [JsonProperty("results")]
        public IEnumerable<CloudMessagingMessageResponse> Results { get; set; }
    }
}
