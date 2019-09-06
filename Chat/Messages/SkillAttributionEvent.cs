using Newtonsoft.Json;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class SkillAttributionEvent : BaseEvent
    {
        public const string EventType = "SkillAttribution";

        [JsonProperty]
        public SkillAttributionData data { get; set; }
        
    }
}