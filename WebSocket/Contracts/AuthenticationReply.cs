using Newtonsoft.Json;

namespace MixerCore.WebSocket.Contracts
{
    [JsonObject]
    public class AuthenticationReply : BaseReply
    {
        [JsonProperty]
        public AuthenticationInfo data { get; set; }

        [JsonProperty]
        public ErrorInfo error { get; set; }
    }
}