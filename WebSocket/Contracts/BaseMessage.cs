using Newtonsoft.Json;

namespace MixerCore.WebSocket.Contracts
{
    [JsonObject]
    public class BaseMessage
    {
        [JsonProperty(PropertyName = "type")]
        public string type { get; set; }
    }
}