using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using Websocket.Client;

namespace EngineIOSharp.Transports
{
    public class WebSocket : Transport
    {
        protected WebsocketClient _webSocket;
        
        public WebSocket(Uri uri, Dictionary<string, string> query = null, bool isSecure = true, string timestampParam = "t", bool timestampRequests = false) : base(uri, query)
        {
            GenerateURI(isSecure, timestampParam, timestampRequests);
            _webSocket = new WebsocketClient(this.uri);
        }

        protected override void OpenConnection()
        {
            _webSocket.Start();
            //throw new System.NotImplementedException();
        }

        protected override void CloseConnection()
        {
            try
            {
                _webSocket.StopOrFail(WebSocketCloseStatus.Empty, "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Emit("error", new ErrorEventArgs("WebSocketException", "Task failed successfully????"));
                throw;
            }
            //throw new System.NotImplementedException();
        }

        protected override void Write(Packet[] packets)
        {
            throw new System.NotImplementedException();
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