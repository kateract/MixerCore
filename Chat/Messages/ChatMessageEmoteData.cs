using Newtonsoft.Json;
using System.Collections.Generic;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class ChatMessageEmoteData : ChatMessageData
    {
        [JsonProperty]
        public string source { get; set; }

        [JsonProperty]
        public string pack { get; set; }

        [JsonProperty]
        public Dictionary<string, uint> coords { get; set; }
    }
}