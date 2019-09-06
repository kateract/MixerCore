using Newtonsoft.Json;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.Chat.Messages
{
    public class ChatPollEndEvent : BaseEvent
    {
        public const string EventType = "PollEnd";

        [JsonProperty]
        public PollInfo data { get; set; }
    }
}