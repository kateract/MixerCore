using Newtonsoft.Json;
using System.Collections.Generic;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class PollInfo
    {
        [JsonProperty]
        public uint originatingChannel { get; set; }

        [JsonProperty]
        public string q { get; set; }

        [JsonProperty]
        public string[] answers { get; set; }

        [JsonProperty]
        public ModeratorData author { get; set; }

        [JsonProperty]
        public uint duration { get; set; }

        [JsonProperty]
        public ulong endsAt { get; set; }

        [JsonProperty]
        public uint voters { get; set; }

        [JsonProperty]
        public Dictionary<string, uint> responses { get; set; }

        [JsonProperty]
        public uint[] responsesByIndex{ get; set; }
    }
}