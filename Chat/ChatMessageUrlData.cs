using Newtonsoft.Json;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class ChatMessageUrlData : ChatMessageData
    {
        [JsonProperty]
        public string url { get; set; }
        
    }
}