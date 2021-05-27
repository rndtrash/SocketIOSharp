using System;
using System.Text;

namespace EngineIOSharp
{
    public struct Packet
    {
        public string Type;
        public dynamic Data;

        public Packet(string type, string data)
        {
            Type = type;
            Data = data;
        }
        
        public Packet(string type, byte[] data)
        {
            Type = type;
            Data = data;
        }
        
        /// <summary>
        /// <c>Encode</c> turns <c>Packet</c> into a byte array
        /// </summary>
        /// <param name="packet">Packet to be encoded</param>
        /// <param name="supportsBinary">Should this function leave the data as is or encode it as Base64 string. Default is <c>false</c></param>
        /// <returns></returns>
        public byte[] Encode(bool supportsBinary = false)
        {
            return Data.GetType() == typeof(byte[])
                ? EncodeBuffer(Data, supportsBinary)
                : Encode(Type, (string) Data);
        }

        private static byte[] Encode(string type, string data)
        {
            return Encoding.ASCII.GetBytes(Parser.PacketTypes[type] + data);
        }

        private static byte[] EncodeBuffer(byte[] data, bool supportsBinary)
        {
            // only 'message' packets can contain binary, so the type prefix is not needed
            return supportsBinary ? data : Encoding.ASCII.GetBytes("b" + Convert.ToBase64String(data));
        }
    }
}