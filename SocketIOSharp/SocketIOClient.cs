using System;
using System.Collections.Generic;
using System.Data;
using EngineIOSharp;
using EngineIOSharp.Transports;

namespace SocketIOSharp
{
    public class SocketIOClient
    {
        public string URL => _URL;
        public ConnectionState ConnectionState => _Transport.ConnState;

        private Transport _Transport;
        private string _URL;

        public SocketIOClient(Uri uri, Dictionary<string, string> query) : this(uri.ToString(), query)
        {
            throw new NotImplementedException();
        }
        
        public SocketIOClient(string url, Dictionary<string, string> query)
        {
            throw new NotImplementedException();
            var ub = new UriBuilder(url);
            _URL = ub.Uri.ToString();
            
            _Transport = new WebSocket(ub.Uri, query);
            //_Transport.Connect();
        }

        ~SocketIOClient()
        {
            throw new NotImplementedException();
            _Transport.Close();
        }
    }
}