using Newtonsoft.Json;

namespace MixerCore.WebSocket.Contracts
{
    [JsonObject]
    public class AuthenticationInfo
    {
        [JsonProperty]
        public bool authenticated { get; set; }
        
        [JsonProperty]
        public string[] roles { get; set; }
    }
}