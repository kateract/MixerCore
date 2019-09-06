using Newtonsoft.Json;

namespace MixerCore.Authentication.Contracts
{
    [JsonObject]
    public class CodeResponse
    {
        [JsonProperty]
        public string code { get; set; }
    }
}