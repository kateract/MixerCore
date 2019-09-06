using Newtonsoft.Json;

namespace MixerCore.WebSocket.Contracts
{
    [JsonObject]
    public class BaseReply : BaseMessage
    {
        public const string Type = "reply";

        [JsonProperty]
        public uint id { get; set; }
    }
}