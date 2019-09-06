using System;

namespace MixerCore.WebSocket.Contracts
{
    public class SocketMethod
    {
        /// <summary>
        /// Authenticate as an active user in a specified channel. Arguments are channelId, userId, key. You can connect anonymously by
        /// supplying just the channnelId as an argument, but if you do this you will not be able to send messages or participate in chat. 
        /// This can be useful for creating chat overlays.
        /// </summary>
        public const string Auth = "auth";

        /// <summary>
        /// A ping method. This should be used in environments that do not support Native WebSocket Pings. An example of this is Chat 
        /// implementations in with a web browser.
        /// </summary>
        public const string Ping = "ping";
    }
}