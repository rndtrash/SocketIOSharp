using System;

namespace EngineIOSharp
{
    public class Client : EventHandler
    {
        public Transport SocketTransport
        {
            get => _socketTransport;
            set
            {
                _socketTransport = null;
                _socketTransport = value;
                SocketTransport.On("drain", OnDrain);
                SocketTransport.On("packet", OnPacket);
                SocketTransport.On("error", OnError);
                SocketTransport.On("close", OnClose);
            }
        }

        private Transport _socketTransport;

        public Client(string url)
        {
            //
        }

        private void OnDrain(object sender, EventArgs e)
        {
            //
        }

        private void OnPacket(object sender, EventArgs e)
        {
            //
        }

        private void OnError(object sender, EventArgs e)
        {
            //
        }

        private void OnClose(object sender, EventArgs e)
        {
            Console.WriteLine("Transport closed");
        }
    }
}