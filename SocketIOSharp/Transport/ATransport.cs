using System;

namespace SocketIOSharp.Transport
{
    public abstract class ATransport
    {
        public ConnectionStatus ConnectionStatus => _cs;
        
        protected SocketIOClient _sio;
        protected ConnectionStatus _cs = ConnectionStatus.DISCONNECTED;

        public ATransport(SocketIOClient sio)
        {
            _sio = sio;
        }

        public abstract void Connect();
        public abstract void Disconnect();
    }
}