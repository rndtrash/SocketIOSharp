using System;
using System.Collections.Specialized;
using SocketIOSharp.Transport;

namespace SocketIOSharp
{
    public class SocketIOClient
    {
        public string URL => _URL;
        public ConnectionStatus ConnectionStatus => _aTransport.ConnectionStatus;

        private ATransport _aTransport;
        private string _URL;

        public SocketIOClient(Uri uri, NameValueCollection query) : this(uri.ToString(), query)
        {
        }
        
        public SocketIOClient(string url, NameValueCollection query)
        {
            var ub = new UriBuilder(url);
            _URL = ub.Uri.ToString();
            
            _aTransport = new SIOWebSocket(this);
            _aTransport.Connect();
        }

        ~SocketIOClient()
        {
            _aTransport.Disconnect();
        }
    }
}