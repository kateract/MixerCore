using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MixerCore.Chat.Http.Contracts;
using MixerCore.Chat.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MixerCore.WebSocket;
using MixerCore.WebSocket.Contracts;

namespace MixerCore.Chat
{
    /// <summary>
    /// Class to connect to mixer as well as sending and recieving chat messages
    /// </summary>
    public class ChatClient : WebSocketClientBase
    {
        /// <summary>
        /// Connects to a chat channel given chat auth info already set up for that channel.
        /// </summary>
        /// <param name="chatInfo">The chat auth and connection info.</param>
        /// <param name="channelId">The channel to connect to.</param>
        /// <param name="userId">The user that is connecting to chat.</param>
        /// <returns>void</returns>
        public async Task ConnectAsync(ChatConnectionInformation chatInfo, uint channelId, uint userId)
        {
            // we have a list of chat servers, the first one is good enough
            server = chatInfo.endpoints[0];

            using (var ts = new CancellationTokenSource())
            {
                //connect the socket and get the welcome message
                await socket.ConnectAsync(new System.Uri(server), ts.Token);
                var messageBytes = await RecieveMessageAsync(ts.Token);
                var welcome = DeserializeMessage<Messages.WelcomeEvent>(messageBytes);
                await Authenticate(chatInfo.authKey, channelId, userId, ts.Token);
            }
        }

        /// <summary>
        /// Get the next chat message from the channel.
        /// </summary>
        /// <returns>The chat message info.</returns>
        public async Task<BaseEvent> GetNextChatMessageAsync()
        {
            BaseEvent chatMessageInfo = null;

            // we'll just keep trying until we get a chat message
            using (var ts = new CancellationTokenSource())
            {
                while (chatMessageInfo == null)
                {
                    try
                    {
                        var nextMessage = await RecieveMessageAsync(ts.Token);

                        var rawJson = DeserializeMessage<JObject>(nextMessage);
                        Console.WriteLine(rawJson.ToString());

                        chatMessageInfo = rawJson.ToObject<BaseEvent>();
                        if (string.CompareOrdinal(chatMessageInfo.type, BaseEvent.Type) != 0)
                        {
                            chatMessageInfo = null;
                        }
                        else if (string.CompareOrdinal(chatMessageInfo.Event, ChatDeleteMessageEvent.EventType) == 0)
                        {
                            chatMessageInfo = rawJson.ToObject<Messages.ChatDeleteMessageEvent>();
                        }
                        else if (string.CompareOrdinal(chatMessageInfo.Event, ChatUserJoinEvent.EventType) == 0)
                        {
                            chatMessageInfo = rawJson.ToObject<Messages.ChatUserJoinEvent>();
                        }
                        else if (string.CompareOrdinal(chatMessageInfo.Event, ChatUserLeaveEvent.EventType) == 0)
                        {
                            chatMessageInfo = rawJson.ToObject<Messages.ChatUserLeaveEvent>();
                        }
                        else if (string.CompareOrdinal(chatMessageInfo.Event, ChatPollStartEvent.EventType) == 0)
                        {
                            chatMessageInfo = rawJson.ToObject<Messages.ChatPollStartEvent>();
                        }
                        else if (string.CompareOrdinal(chatMessageInfo.Event, ChatPollEndEvent.EventType) == 0)
                        {
                            chatMessageInfo = rawJson.ToObject<Messages.ChatPollEndEvent>();
                        }
                        else if (string.CompareOrdinal(chatMessageInfo.Event, ChatMessageEvent.EventType) == 0)
                        {
                            //chatMessageInfo = rawJson.ToObject<Messages.ChatMessageEvent>();
                            var chatEvent = new ChatMessageEvent
                            {
                                data = new ChatMessageInfo()
                                {
                                    id = rawJson["data"]["id"].ToString(),
                                    user_id = uint.Parse(rawJson["data"]["user_id"].ToString()),
                                    user_name = rawJson["data"]["user_name"].ToString()
                                }
                            };
                            var messages = rawJson["data"]["message"]["message"].Children();
                            chatEvent.data.message = new ChatMessagesData();
                            foreach(var result in messages)
                            {
                                var data = result.ToObject<ChatMessageData>();
                                switch (data.type)
                                {
                                    case ChatMessageType.emoticon:
                                        chatEvent.data.message.message.Add(result.ToObject<ChatMessageEmoteData>());
                                        break;
                                    case ChatMessageType.tag:
                                        chatEvent.data.message.message.Add(result.ToObject<ChatMessageTagData>());
                                        break;
                                    case ChatMessageType.image:
                                    case ChatMessageType.link:
                                        chatEvent.data.message.message.Add(result.ToObject<ChatMessageUrlData>());
                                        break;
                                    default:
                                        chatEvent.data.message.message.Add(data);
                                        break;
                                }

                            }
                            chatMessageInfo = chatEvent;
                        }
                        else
                        {
                            // we didn't match a type we care about, just throw it away
                            chatMessageInfo = null;
                        }
                    }
                    catch (Exception ex) 
                    {
                        Console.Error.WriteLine(ex.Message);
                        // if anything goes wrong kill the message
                        chatMessageInfo = null;
                    }
                }
            }

            return chatMessageInfo;
        }



        /// <summary>
        /// Send a whisper to another user in the chat.
        /// </summary>
        /// <see cref="https://dev.mixer.com/reference/chat/methods/whisper"/>
        /// <param name="targetUserName">The user to whisper to</param>
        /// <param name="message">The message to send to the user</param>
        /// <returns>void</returns>
        public async Task SendWhisperAsync(string targetUserName, string message) => await SendToChatAsync(ChatMethod.Whisper, new JArray(targetUserName, message));

        /// <summary>
        /// Delete a message out of the channel's chat.
        /// </summary>
        /// <see cref="https://dev.mixer.com/reference/chat/methods/deletemessage"/>
        /// <param name="messageId">The id of the message to delete</param>
        /// <returns>void</returns>
        public async Task SendDeleteMessageAsync(string messageId) => await SendToChatAsync(ChatMethod.DeleteMessage, new JArray(messageId));

        /// <summary>
        /// Send a chat message to the server. The server will reply with data identical to a ChatMessage event.
        /// </summary>
        /// <see cref="https://dev.mixer.com/reference/chat/methods/msg"/>
        /// <param name="message">The message to send to chat</param>
        /// <returns>void</returns>
        public async Task SendMessageAsync(string message) => await SendToChatAsync(ChatMethod.Message, new JArray(message));

        /// <summary>
        /// Call to do the socket send.
        /// </summary>
        /// <param name="method">The method to be sent</param>
        /// <param name="parameters">Parameters to the method</param>
        /// <returns>Async task</returns>
        private async Task SendToChatAsync(string method, JArray parameters)
        {
            using (var ts = new CancellationTokenSource())
            {
                var methodData = new SendMethodCall
                {
                    method = method,
                    arguments = parameters,
                    id = Interlocked.Increment(ref messageId),
                };

                await socket.SendAsync(SerializeToJsonBytes(methodData), WebSocketMessageType.Text, true, ts.Token);

                // at this point you'd want to validate the response, but we're throwing them away
                // in the recieve for simplicity, so we'll skip it
            }
        }



    }
}