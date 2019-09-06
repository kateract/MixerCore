using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MixerCore.WebSocket.Contracts
{
    [JsonObject]
    public class SendMethodCall
    {
        [JsonProperty]
        public string type { get; } = "method";

        [JsonProperty]
        public string method { get; set; }

        [JsonProperty]
        public JArray arguments { get; set; }

        [JsonProperty]
        public long id { get; set; }
        
    }
}