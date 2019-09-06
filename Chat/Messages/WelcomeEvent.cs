using Newtonsoft.Json;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class WelcomeEvent : BaseEvent
    {
        public const string EventType = "WelcomeEvent";

        [JsonProperty]
        public ConnectionInfo data { get; set; }
    }
}