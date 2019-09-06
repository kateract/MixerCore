using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.WebSocket
{
    public class WebSocketClientBase
    {
        protected ClientWebSocket socket = new ClientWebSocket();
        protected string server = null;
        protected JsonSerializer serializer = JsonSerializer.Create();
        protected long messageId = 0;

                /// <summary>
        /// Send the user authentication info to the chat server
        /// </summary>
        /// <see cref="https://dev.mixer.com/reference/chat/methods/auth"/>
        /// <param name="authToken">The chat auth token</param>
        /// <param name="channelId">The channel to connect to chat for</param>
        /// <param name="userId">The user the auth token is for</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>void</returns>
        protected async Task Authenticate(string authToken, uint channelId, uint userId, CancellationToken token)
        {
            var methodData = new Contracts.SendMethodCall
            {
                method = SocketMethod.Auth,
                arguments = new JArray(channelId, userId, authToken),
                id = Interlocked.Increment(ref messageId),
            };

            await socket.SendAsync(SerializeToJsonBytes(methodData), WebSocketMessageType.Text, true, token);

            // the response may not be the next message
            AuthenticationReply reply = null;
            while (reply == null)
            {
                var nextMessage = await RecieveMessageAsync(token);
                reply = DeserializeMessage<AuthenticationReply>(nextMessage);
                if (string.CompareOrdinal(reply.type, BaseReply.Type) != 0 || reply.id != methodData.id)
                {
                    reply = null;
                }
            }
            
            if (reply.error != null)
            {
                throw new WebSocketException(reply.error.code, reply.error.message);
            }
        }

        /// <summary>
        /// Get the next message from the chat server. This is where buffer management happens.
        /// </summary>
        /// <param name="token">A cancellation token.</param>
        /// <returns>The message bytes or null.</returns>
        protected async Task<byte[]> RecieveMessageAsync(CancellationToken token)
        {
            // First we'll need a buffer to get the data from the socket into
            const int recvBufferSize = 1024 * 12;
            var buffer = new byte[recvBufferSize];

            // Websocket uses an array segment to manage that buffer
            var segment = new ArraySegment<byte>(buffer);

            // Recieve will wait for the next message to come in 
            var result = await socket.ReceiveAsync(segment, token);

            // Receive may not have gotten all of the message
            var offset = result.Count;
            while(!result.EndOfMessage)
            {
                // we may have run out of space in the buffer
                if (offset >= buffer.Length)
                {
                    Array.Resize(ref buffer, buffer.Length + recvBufferSize);
                }

                // Reset the ArraySegment at the new offset and get more data
                segment = new ArraySegment<byte>(buffer, offset, buffer.Length - offset);
                result = await socket.ReceiveAsync(segment, token);
                offset += result.Count;
            }

            // Maybe there was no data, a cancel, or timeout
            if (offset == 0)
            {
                return null;
            }

            // now we need to return the message in the right sized byte array
            var message = new byte[offset];
            Buffer.BlockCopy(buffer, 0, message, 0, offset);
            return message;
        }

        /// <summary>
        /// Serializes an object to bytes that can be sent to the chat server.
        /// </summary>
        /// <typeparam name="T">The type of object to send.</typeparam>
        /// <param name="obj">The data to serialize.</param>
        /// <returns>Date that can be sent to the chat server.</returns>
        public ArraySegment<byte> SerializeToJsonBytes<T>(T obj)
        {
            var memoryStream = new MemoryStream();

            var sw = new StreamWriter(memoryStream);
            using (var jtw = new JsonTextWriter(sw))
            {
                serializer.Serialize(jtw, obj);
                sw.Flush();
                memoryStream.Position = 0;
                return new ArraySegment<byte>(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Deserializes a byte stream from the socket into objects.
        /// </summary>
        /// <typeparam name="T">The expected object type of the message.</typeparam>
        /// <param name="bytes">The message from the socket.</param>
        /// <returns>A deserialized object.</returns>
        public T DeserializeMessage<T>(byte[] bytes)
        {
            var memoryStream = new MemoryStream(bytes);
            memoryStream.Flush();
            memoryStream.Position = 0;
            var sr = new StreamReader(memoryStream);
            using (var jr = new JsonTextReader(sr))
            {                
                return serializer.Deserialize<T>(jr);
            }
        }
    }

    
}