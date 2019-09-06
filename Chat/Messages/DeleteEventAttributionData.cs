using Newtonsoft.Json;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class DeleteEventAttributionData
    {
        [JsonProperty]
        public ModeratorData moderator { get; set; }

        [JsonProperty]
        public string id { get; set; }
        
    }
}