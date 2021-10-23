using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketsExample
{
    class Program
    {
        private static WebSocketServer _wsServer;
        private static ConcurrentDictionary<string, byte> _sessions = new();

        static void Main(string[] args)
        {
            // https://github.com/sta/websocket-sharp
            _wsServer = new WebSocketServer(4649);
            _wsServer.AddWebSocketService<WSDeals>("/deals"); // Url: ws://localhost:4649/deals
            _wsServer.Start();
            Console.ReadKey(true);

            SendToAllSessions("some text");
            _wsServer.Stop();
        }

        public class WSDeals : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e) // process message from client
            {
                _sessions.AddOrUpdate(ID, 0, (key, oldValue) => { return 0; }); // save sessionId

                var msg = e.Data == "abc" ? "response1" : "response2";
                Send(msg); // respond to client
            }
        }

        public static WebSocketServiceHost GetEndpoint() =>
            _wsServer.WebSocketServices["/deals"];

        public static void SendToSession(string sessionId, string data) // custom message from server to client
        {
            GetEndpoint().Sessions.SendTo(data, sessionId);
        }
        public static void SendToAllSessions(string data) // custom message from server to all clients
        {
            foreach (var sessionId in _sessions.Keys)
                SendToSession(sessionId, data); 
        }

        public static List<string> GetActiveSessions()
        {
            var sessions = GetEndpoint().Sessions.ActiveIDs;
            return sessions.ToList();
        }
    }
}
