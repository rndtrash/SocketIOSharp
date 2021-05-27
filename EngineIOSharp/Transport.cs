using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EngineIOSharp
{
    public abstract class Transport : EventHandler
    {
        public bool Writable => _writable;
        public ConnectionState ConnState => _connectionState;

        protected Uri uri;
        protected Dictionary<string, string> query;

        private ConnectionState _connectionState = ConnectionState.Closed;
        private bool _writable = false;

        protected Transport(Uri uri, Dictionary<string, string> query = null)
        {
            this.query = query ?? new Dictionary<string, string>();
            this.uri = uri;
            
            this.query.Add("EIO", Parser.Protocol.ToString());
            
            Events.Add("open", null);
            // Events.Add("drain", null);
            Events.Add("packet", null);
            Events.Add("error", null);
            Events.Add("close", null);
        }

        public void Error(string type, string description)
        {
            Emit("error", new ErrorEventArgs(type, description));
        }

        public void Open()
        {
            if (ConnState != ConnectionState.Closed)
                return;

            _connectionState = ConnectionState.Connecting;
            OpenConnection();

            _connectionState = ConnectionState.Open;
            _writable = true;
            Emit("open", EventArgs.Empty);
        }

        protected abstract void OpenConnection();

        public void Close()
        {
            if (ConnState == ConnectionState.Connecting || ConnState == ConnectionState.Connecting)
                return;

            CloseConnection();
            
            _connectionState = ConnectionState.Closed;
            Emit("close", EventArgs.Empty);
        }

        protected abstract void CloseConnection();

        public void Send(Packet[] packets)
        {
            if (ConnState == ConnectionState.Open)
                Write(packets);
        }

        protected abstract void Write(Packet[] packets);

        protected void OnData(string data)
        {
            var packet = Parser.DecodePacket(data);
            OnPacket(packet);
        }

        protected void OnPacket(Packet packet)
        {
            Emit("packet", new PacketEventArgs(packet));
        }

        protected static string MakeQuery(Dictionary<string, string> query)
        {
            var pairs = new string[query.Count];

            var i = 0;
            foreach (var q in query)
            {
                pairs[i++] = HttpUtility.UrlEncode(q.Key) + "=" + HttpUtility.UrlEncode(q.Value);
            }
            
            return "?" + string.Join("&", pairs);
        }

    }
}