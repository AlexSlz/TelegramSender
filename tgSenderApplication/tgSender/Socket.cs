using System.Text;
using WatsonWebsocket;


namespace tgSender
{
    internal class Socket
    {
        static WatsonWsServer server = new WatsonWsServer("localhost", 80, false);
        static string clientPort;
        public static void Server()
        {
            server.ClientConnected += ClientConnected;
            server.Start();
        }
        static void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            clientPort = args.IpPort;
        }
        static string jsonData = "{\"tgData\":[]}";
        static int jsonCount = 0;
        public static void AddJson(string data)
        {
            jsonData = jsonData.Remove(jsonData.Length - 2, 2);
            jsonData += data + "]}";
            jsonCount++;

        }
        public static void SendData(string data)
        {
            server.SendAsync(clientPort, data);
        }
        public static void SendData()
        {
            if(jsonCount > 0)
            jsonData = jsonData.Remove(jsonData.Length - 3, 1);
            server.SendAsync(clientPort, Encoding.UTF8.GetBytes(jsonData));
        }
    }
}
