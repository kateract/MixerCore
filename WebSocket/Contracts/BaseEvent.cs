using Newtonsoft.Json;

namespace MixerCore.WebSocket.Contracts
{
    [JsonObject]
    public class BaseEvent : BaseMessage
    {
        public const string Type = "event";

        [JsonProperty(PropertyName = "event")]
        public string Event { get; set; }
    }
}