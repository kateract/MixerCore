using Newtonsoft.Json;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class ChatDeleteMessageEvent: BaseEvent
    {
        public const string EventType = "DeleteMessage";

        [JsonProperty]
        public DeleteEventAttributionData data { get; set; }
        
    }
}