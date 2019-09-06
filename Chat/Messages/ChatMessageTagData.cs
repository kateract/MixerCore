using Newtonsoft.Json;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class ChatMessageTagData : ChatMessageData
    {
        
        [JsonProperty]
        public string username { get; set; }

        [JsonProperty]
        public uint id { get; set; }
    }
}