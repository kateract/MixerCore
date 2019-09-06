using Newtonsoft.Json;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class ConnectionInfo
    {
        [JsonProperty]
        public string server { get; set; }
        
    }
}