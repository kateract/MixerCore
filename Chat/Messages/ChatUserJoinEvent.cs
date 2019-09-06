using Newtonsoft.Json;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class ChatUserJoinEvent : BaseEvent
    {
        public const string EventType = "UserJoin";

        [JsonProperty]
        public ChatUserData data { get; set; }
    }
}