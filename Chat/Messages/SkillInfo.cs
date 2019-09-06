using Newtonsoft.Json;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class SkillInfo
    {
        [JsonProperty]
        public string skill_id { get; set; }

        [JsonProperty]
        public string skill_name { get; set; }

        [JsonProperty]
        public string execution_id { get; set; }

        [JsonProperty]
        public string icon_url { get; set; }

        [JsonProperty]
        public uint cost { get; set; }

        [JsonProperty]
        public string currency { get; set; }
    }
}