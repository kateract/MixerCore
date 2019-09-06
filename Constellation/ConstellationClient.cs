using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using MixerCore.WebSocket;
using MixerCore.WebSocket.Contracts;
using MixerCore.Constellation.Contracts;

namespace MixerCore.Constellation
{
    public class ConstellationClient : WebSocketClientBase
    {
        public ConstellationClient() : base()
        {
            server = "wss:////constellation.mixer.com";
        }

        public async Task ConnectAsync(ConstellationConnectionInformation constellationConnectionInformation, uint channelId, uint userId)
        {
            using (var ts = new CancellationTokenSource())
            {
                
                await socket.ConnectAsync(new System.Uri(server), ts.Token);

            }
        }

    }

}
