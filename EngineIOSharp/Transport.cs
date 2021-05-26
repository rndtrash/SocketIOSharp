using System;
using System.Collections.Generic;
using System.Data;

namespace EngineIOSharp
{
    public abstract class Transport : EventHandler
    {
        public bool Writable => _Writable;
        
        protected ConnectionState State = ConnectionState.Closed;
        protected bool _Writable;
        
        protected Transport(Dictionary<string, string> query)
        {
            Events.Add("open", null);
            Events.Add("drain", null);
            Events.Add("packet", null);
            Events.Add("error", null);
            Events.Add("close", null);
        }

        protected void Error(string type, string description)
        {
            Emit("error", new ErrorEventArgs(type, description));
        }

        public void Open()
        {
            if (State != ConnectionState.Closed)
                return;

            State = ConnectionState.Connecting;
            OpenConnection();

            State = ConnectionState.Open;
            _Writable = true;
            Emit("open", EventArgs.Empty);
        }

        protected abstract void OpenConnection();

        public void Close()
        {
            if (State == ConnectionState.Connecting || State == ConnectionState.Connecting)
                return;

            CloseConnection();
            
            State = ConnectionState.Closed;
            Emit("close", EventArgs.Empty);
        }

        protected abstract void CloseConnection();
    }
}