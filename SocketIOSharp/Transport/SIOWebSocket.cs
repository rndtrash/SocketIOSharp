using System;
using WebSocket = WebSocketSharp.WebSocket;

namespace SocketIOSharp.Transport
{
    public class SIOWebSocket : ATransport
    {
        private WebSocket _ws;

        public SIOWebSocket(SocketIOClient sio) : base(sio)
        {
        }

        public override void Connect()
        {
            _ws = new WebSocket(_sio.URL);
            _ws.OnOpen += OnOpen;
            _ws.OnClose += OnClose;
            _ws.Connect();
        }

        public override void Disconnect()
        {
            _ws.Close();
        }

        private void OnOpen(object sender, EventArgs e)
        {
            _cs = ConnectionStatus.CONNECTED;
        }

        private void OnClose(object sender, EventArgs e)
        {
            _cs = ConnectionStatus.DISCONNECTED;
        }
    }
}