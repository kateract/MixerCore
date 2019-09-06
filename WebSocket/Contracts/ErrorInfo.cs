using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MixerCore.WebSocket.Contracts
{
   [JsonObject]
    public class ErrorInfo
    {
        [JsonProperty]
        public int code { get; set; }

        [JsonProperty]
        public string message { get; set; }

        [JsonProperty]
        public string stacktrace { get; set; }

        [JsonProperty]
        public JObject data { get; set; }

    }
}