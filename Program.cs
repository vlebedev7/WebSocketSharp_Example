using System;
using System.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var wsServer = new WebSocketServer(4649);
            wsServer.AddWebSocketService<WSDeals>("/deals");
            wsServer.Start();
            Console.ReadKey();
            wsServer.Stop();
        }

        public class WSDeals : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                var msg = e.Data == "abc" ? "response1" : "response2";
                Send(msg);
            }
        }
    }
}
