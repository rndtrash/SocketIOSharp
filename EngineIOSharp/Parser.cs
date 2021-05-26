using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineIOSharp
{
    public static class Parser
    {
        public static readonly uint Protocol = 4;
        
        public static readonly Dictionary<string, string> PacketTypes = new Dictionary<string, string>
        {
            {"open", "0"},
            {"close", "1"},
            {"ping", "2"},
            {"pong", "3"},
            {"message", "4"},
            {"upgrade", "5"},
            {"noop", "6"}
        };

        public static readonly Dictionary<string, string> PacketTypesReverse =
            PacketTypes.ToDictionary(x => x.Value, x => x.Key);

        public static readonly Packet ErrorPacket = new Packet("error", "parser error");

        public static readonly char Separator = Convert.ToChar(30);

        public static Packet DecodePacket(byte[] encodedPacket)
        {
            return new Packet(
                    "message",
                    encodedPacket
                    );
        }
        
        public static Packet DecodePacket(string encodedPacket)
        {
            var type = encodedPacket[0].ToString();
            if (type == "b")
            {
                var buffer = Convert.FromBase64String(encodedPacket[1..]);
                return new Packet(
                    "message",
                    buffer
                );
            }

            if (!PacketTypesReverse.ContainsKey(type))
                return ErrorPacket;

            return (encodedPacket.Length > 1)
                ? new Packet(PacketTypesReverse[type], encodedPacket[1..])
                : new Packet(PacketTypesReverse[type], (byte[]) null);
        }

        public static string EncodePayload(Packet[] packets)
        {
            var result = "";

            for (var i = 0; i < packets.Length; i++)
            {
                result += Encoding.ASCII.GetString(packets[i].Encode(false));
                if (i != packets.Length - 1)
                    result += Separator;
            }
            
            return result;
        }

        public static List<Packet> DecodePayload(string encodedPayload)
        {
            var encodedPackets = encodedPayload.Split(Separator);
            var decodedPackets = new List<Packet>();

            foreach (var s in encodedPackets)
            {
                var decoded = DecodePacket(s);
                if (decoded.Type == ErrorPacket.Type)
                    break;
                decodedPackets.Add(decoded);
            }

            return decodedPackets;
        }
    }
}