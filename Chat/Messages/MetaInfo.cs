using Newtonsoft.Json;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class MetaInfo
    {
        [JsonProperty]
        public bool me { get; set; }

        [JsonProperty]
        public bool is_skill { get; set; }

        [JsonProperty]
        public SkillInfo skill { get; set; }
    }
}