using Newtonsoft.Json;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class ChatUserLeaveEvent : BaseEvent
    {
        public const string EventType = "UserLeave";

        [JsonProperty]
        public ChatUserData data { get; set; }
        
    }
}