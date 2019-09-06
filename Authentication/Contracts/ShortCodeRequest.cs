using Newtonsoft.Json;

namespace MixerCore.Authentication.Contracts
{
    [JsonObject]
    public class ShortCodeRequest
    {
        [JsonProperty]
        public string client_id { get; set; }

        [JsonProperty]
        public string client_secret { get; set; }

        [JsonProperty]
        public string scope { get; set; }
    }
}