using System;
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
            Transport t = new WebSocket(new Uri("http://scotland-club.herokuapp.com/socket.io"), timestampRequests: true);
            t.Open();
            t.Close();
            Assert.IsTrue(true, "TODO");
        }
    }
}