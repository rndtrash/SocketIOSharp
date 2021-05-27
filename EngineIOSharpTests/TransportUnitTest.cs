using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using EngineIOSharp;
using EngineIOSharp.Transports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EngineIOSharpTests
{
    [TestClass]
    public class TransportUnitTest
    {
        [TestMethod]
        public void WebSocket()
        {
            var sourcePacket = new Packet("message", "test");
            
            Transport t = new WebSocket(// new Uri("http://scotland-club.herokuapp.com/socket.io"),
                new Uri("ws://echo.websocket.org"),
                new Dictionary<string, string> {{"nickname", "scotland"}},
                false,
                timestampRequests: true);
            
            t.On("packet", (object sender, EventArgs args) =>
            {
                var pea = (PacketEventArgs) args;
                Assert.IsTrue(
                    Encoding.ASCII.GetString(pea.packet.Encode(false)) ==
                    Encoding.ASCII.GetString(sourcePacket.Encode(false)),
                    "Received and decoded packet should be equal to source packet");
                t.Close();
            });
            
            t.On("error", (object sender, EventArgs args) =>
            {
                if (args == null)
                    return;
                var eea = (ErrorEventArgs) args;
                Console.WriteLine("ERROR: " + eea.type + ": " + eea.description);
                Assert.Fail("There should not be any errors");
            });

            var task = Task.Run(() => {
                t.Open();
                t.Send(new[] {
                    sourcePacket
                });
                while (t.ConnState != ConnectionState.Closed) {}
            });
            
            if (!task.Wait(TimeSpan.FromSeconds(15)))
            {
                if (t.ConnState != ConnectionState.Closed)
                    Assert.Fail("Request timeout");
            }
        }
    }
}