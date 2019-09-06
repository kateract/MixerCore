using Newtonsoft.Json;

namespace MixerCore.Authentication.Contracts
{
    [JsonObject]
    public class RefreshTokenRequest
    {
        [JsonProperty]
        public string grant_type { get; } = "refresh_token";

        [JsonProperty]
        public string refresh_token { get; set; }

        [JsonProperty]
        public string client_id { get; set; }

        [JsonProperty]
        public string client_secret { get; set; }
    }
}