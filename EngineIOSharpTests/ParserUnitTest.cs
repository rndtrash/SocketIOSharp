using System;
using System.Text;
using EngineIOSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EngineIOSharpTests
{
    [TestClass]
    public class ParserUnitTest
    {
        private bool IsEqual(byte[] l, byte[] r)
        {
            if (l.Length != r.Length)
                return false;
            
            for (var i = 0; i < l.Length; i++)
            {
                if (l[i] != r[i])
                    return false;
            }
            
            return true;
        }
        
        private bool IsEqual(Packet[] l, Packet[] r)
        {
            if (l.Length != r.Length)
                return false;
            
            for (var i = 0; i < l.Length; i++)
            {
                if (!IsEqual(l[i], r[i]))
                    return false;
            }
            
            return true;
        }

        private bool IsEqual(Packet l, Packet r)
        {
            return l.Type == r.Type && l.Data.GetType() == r.Data.GetType() &&
                   (l.Data.GetType() is not byte[] || IsEqual((byte[]) l.Data, (byte[]) r.Data)) &&
                   (l.Data.GetType() is not string || (string) l.Data == (string) r.Data);
        }
        
        [TestMethod]
        public void SinglePacketAsByteArray()
        {
            var sourceData = new byte[] {0, 1, 2, 3};
            var sourcePacket = new Packet(
                "message",
                sourceData);

            var encodedPacket = sourcePacket.Encode(true);
            Assert.IsTrue(encodedPacket.Equals(sourceData), "Source data should be equal to encoded \"message\" packet");

            var decodedPacket = Parser.DecodePacket(encodedPacket);
            Assert.IsTrue(decodedPacket.Type == sourcePacket.Type, "Type of decoded packet should be equal to the type of source packet");
            Assert.IsTrue(IsEqual(sourcePacket.Data, decodedPacket.Data), "Data of decoded packet should be equal to the data of source packet");
        }

        [TestMethod]
        public void SinglePacketAsBase64Encoded()
        {
            var sourceData = new byte[] {0, 1, 2, 3};
            var sourcePacket = new Packet(
                "message",
                sourceData);
            
            var encodedPacket = Encoding.Default.GetString(sourcePacket.Encode(false));
            Assert.IsTrue(encodedPacket == "b" + Convert.ToBase64String(sourceData));
            
            var decodedPacket = Parser.DecodePacket(encodedPacket);
            Assert.IsTrue(decodedPacket.Type == sourcePacket.Type, "Type of decoded packet should be equal to the type of source packet");
            Assert.IsTrue(IsEqual(sourcePacket.Data, decodedPacket.Data), "Data of decoded packet should be equal to the data of source packet");
        }

        [TestMethod]
        public void Payload()
        {
            var sourcePackets = new Packet[]
            {
                new("message", "test"),
                new("message", new byte[] {1, 2, 3, 4})
            };
            var payload = Parser.EncodePayload(sourcePackets);
            Assert.IsTrue(payload == "4test" + "\x1e" + "bAQIDBA==");
            
            var decodedPackets = Parser.DecodePayload(payload).ToArray();
            Assert.IsTrue(IsEqual(decodedPackets, sourcePackets));
        }
    }
}