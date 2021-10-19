using System;
using System.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketsExample
{
    class Program
    {
        private static WebSocketServer _wsServer;

        static void Main(string[] args)
        {
            _wsServer = new WebSocketServer(4649);
            _wsServer.AddWebSocketService<WSDeals>("/deals"); // Url: ws://localhost:4649/deals
            _wsServer.Start();
            Console.ReadKey(true);

            Send("4444");
            _wsServer.Stop();
        }

        public class WSDeals : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e) // process message from client
            {
                var msg = e.Data == "abc" ? "response1" : "response2";
                Send(msg); // respond to client
            }
        }

        public static void Send(string data) // custom message from server to client
        {
            var endpoint = _wsServer.WebSocketServices["/deals"];
            var sessionId = endpoint.Sessions.ActiveIDs.First();
            endpoint.Sessions.SendTo(data, sessionId);
        }
    }
}
