using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using Websocket.Client;

namespace EngineIOSharp.Transports
{
    public class WebSocket : Transport
    {
        private readonly WebsocketClient _webSocket;
        
        public WebSocket(Uri uri, Dictionary<string, string> query = null, bool isSecure = true, string timestampParam = "t", bool timestampRequests = false) : base(uri, query)
        {
            GenerateURI(isSecure, timestampParam, timestampRequests);
            _webSocket = new WebsocketClient(this.uri);
            
            // TODO: add remaining events for _webSocket
            _webSocket.MessageReceived.Subscribe(delegate(ResponseMessage message)
            {
                switch (message.MessageType)
                {
                    case WebSocketMessageType.Binary:
                        OnData(message.Binary);
                        break;
                    case WebSocketMessageType.Text:
                        OnData(message.Text);
                        break;
                }
            });
            _webSocket.DisconnectionHappened.Subscribe(delegate(DisconnectionInfo info)
            {
                if (info.Type == DisconnectionType.Error)
                {
                    Console.WriteLine(info.Exception);
                    Error(info.Type.ToString(), info.Exception.Message);
                }

                CloseConnection();
            });
        }

        protected override void OpenConnection()
        {
            try
            {
                _webSocket.StartOrFail();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override void CloseConnection()
        {
            if (!_webSocket.IsRunning)
                return;
            
            try
            {
                _webSocket.StopOrFail(WebSocketCloseStatus.Empty, "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Emit("error", new ErrorEventArgs("WebSocketException", "Task failed successfully????"));
            }
        }

        protected override void Write(Packet[] packets)
        {
            _writable = false;
            
            foreach (var packet in packets)
            {
                var encoded = packet.Encode(true);
                _webSocket.Send(encoded);
            }

            _writable = true;
            Emit("drain");
        }

        private void GenerateURI(bool isSecure, string timestampParam, bool timestampRequests)
        {
            if (timestampRequests)
                query.Add(timestampParam, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
            
            var uriBuilder = new UriBuilder
            {
                Scheme = isSecure ? "wss" : "ws",
                Port = uri.IsDefaultPort ? isSecure ? 443 : 80 : uri.Port,
                Host = uri.Host,
                Path = uri.LocalPath,
                Fragment = uri.Fragment,
                Query = MakeQuery(query)
            };

            uri = uriBuilder.Uri;
        }
    }
}