using System;
using System.Collections.Generic;

namespace EngineIOSharp.Transports
{
    public class Polling : Transport
    {
        public Polling(Uri uri, Dictionary<string, string> query) : base(uri, query)
        {
        }

        protected override void OpenConnection()
        {
            throw new System.NotImplementedException();
        }

        protected override void CloseConnection()
        {
            throw new System.NotImplementedException();
        }

        protected override void Write(Packet[] packets)
        {
            throw new System.NotImplementedException();
        }
    }
}