using System;
using System.Runtime.CompilerServices;

namespace EngineIOSharp
{
    public class ErrorEventArgs : EventArgs
    {
        public string type;
        public string description;

        public ErrorEventArgs(string type, string description)
        {
            this.type = type;
            this.description = description;
        }
    }
}