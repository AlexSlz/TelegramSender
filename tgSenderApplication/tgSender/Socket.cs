using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;
using System;
using System.Threading;

namespace tgSender
{
    internal class Socket
    {
        static WatsonWsServer server = new WatsonWsServer("localhost", 80, false);
        static string clientPort = "";
        public static void Server()
        {
            server.ClientConnected += ClientConnected;
            server.Start();
        }
        static void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            //Console.WriteLine("Connect To Browser | ok");
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
        public async static Task<string> SendBack(string[] param){
            jsonData = param[0];
            return "OK";
        }
        public static void SendData()
        {
            if(jsonCount > 0)
            jsonData = jsonData.Remove(jsonData.Length - 3, 1);
            for (int i = 0; i < 10; i++)
            {
                if (clientPort != "")
                {
                    server.SendAsync(clientPort, Encoding.UTF8.GetBytes(jsonData));
                    break;
                }
                Thread.Sleep(100);
            }
        }
    }
}
