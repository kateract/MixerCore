using Newtonsoft.Json;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class ChatMessageEvent : BaseEvent
    {
        public const string EventType = "ChatMessage";

        [JsonProperty]
        public ChatMessageInfo data { get; set; }
        
    }
}