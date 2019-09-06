using Newtonsoft.Json;

namespace MixerCore.Chat.Rest.Contracts
{
    [JsonObject]
    public class ChatConnectionInformation
    {
        [JsonProperty]
        public string[] roles { get; set; }

        [JsonProperty]
        public string authKey { get; set; }

        [JsonProperty]
        public string[] permissions { get; set; }

        [JsonProperty]
        public string[] endpoints { get; set; }
    }
}