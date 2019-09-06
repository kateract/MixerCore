using Newtonsoft.Json;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class SkillAttributionData
    {
        [JsonProperty]
        public string id { get; set; }

        [JsonProperty]
        public SkillInfo skill { get; set; }

        [JsonProperty]
        public uint user_id { get; set; }

        [JsonProperty]
        public string user_name { get; set; }

        [JsonProperty]
        public string[] user_roles { get; set; }

        [JsonProperty]
        public uint user_level { get; set; }

        [JsonProperty]
        public string user_avatar { get; set; }
    }
}