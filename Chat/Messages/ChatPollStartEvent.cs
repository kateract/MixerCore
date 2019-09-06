using Newtonsoft.Json;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.Chat.Messages
{
    public class ChatPollStartEvent : BaseEvent
    {
        public const string EventType = "PollStart";

        [JsonProperty]
        public PollInfo data { get; set; }
    }
}