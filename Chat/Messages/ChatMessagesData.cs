using System.Collections.Generic;
using Newtonsoft.Json;

namespace MixerCore.Chat.Messages
{
    [JsonObject]
    public class ChatMessagesData
    {
        public IList<ChatMessageData> message { get; } = new List<ChatMessageData>();
    }
}