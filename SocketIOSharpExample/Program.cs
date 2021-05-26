using System;
using System.Collections.Specialized;
using SocketIOSharp;

namespace SocketIOSharpExample
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var sio = new SocketIOClient("wss://scotland-club.herokuapp.com/socket.io/", new NameValueCollection {{"nickname", "scotland"}});
            
            while (true) {}
        }
    }
}