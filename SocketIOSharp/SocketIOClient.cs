using System;
using System.Collections.Specialized;
using System.Data;
using EngineIOSharp;

namespace SocketIOSharp
{
    public class SocketIOClient
    {
        public string URL => _URL;
        public ConnectionState ConnectionState => _Transport.ConnState;

        private Transport _Transport;
        private string _URL;

        public SocketIOClient(Uri uri, NameValueCollection query) : this(uri.ToString(), query)
        {
        }
        
        public SocketIOClient(string url, NameValueCollection query)
        {
            var ub = new UriBuilder(url);
            _URL = ub.Uri.ToString();
            
            //_Transport = new SIOWebSocket(this);
            //_Transport.Connect();
        }

        ~SocketIOClient()
        {
            _Transport.Close();
        }
    }
}