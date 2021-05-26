using System;
using System.Collections.Generic;

namespace EngineIOSharp
{
    public class WebSocket : Transport
    {
        public WebSocket(Dictionary<string, string> query) : base(query)
        {
            throw new NotImplementedException();
        }

        protected override void OpenConnection()
        {
            throw new NotImplementedException();
        }

        protected override void CloseConnection()
        {
            throw new NotImplementedException();
        }
    }
}