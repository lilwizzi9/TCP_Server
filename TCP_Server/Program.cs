using System;

using SimpleTCP;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

using System.Diagnostics;

namespace TCP_Server
{
    internal class Program
    {
        static Stopwatch sw;
        static void Main(string[] args)
        {
            System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse("0.0.0.0");
            UdpClient udpServer = new UdpClient(5508);

            var Server = new SimpleTcpServer();



            Server.ClientConnected += Server_ClientConnected;
            Server.ClientDisconnected += client_Disconnected;
            Server.DataReceived += Server_DataReceived;
            sw = new Stopwatch();
            sw.Start();

            Server.Start(ipaddress, 5080);

            try
            {


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            while (true)
            {
                //var remoteEP = new IPEndPoint(IPAddress.Any, 5508);
                //udpServer.Receive(ref remoteEP);
                //var data = Encoding.UTF8.GetString(udpServer.Receive(ref remoteEP));
                //Console.WriteLine("receive data from " + remoteEP.ToString()+" : "+data);

                if (Server.ConnectedClientsCount > 0)
                {
                    Console.WriteLine($"Handle Has {Server.ConnectedClientsCount} Connection");
                }
                else
                {
                    Console.WriteLine("Waiting For Connection...");
                }
                Thread.Sleep(500);

            }
        }

        private static void Server_DataReceived(object sender, Message e)
        {
            var msg = Encoding.UTF8.GetString(e.Data);
            sw.Stop();
            Console.Clear();
            Console.WriteLine("Elapsed: " + sw.Elapsed);
            sw.Restart();
            Console.WriteLine($"{e.TcpClient.Client.RemoteEndPoint}: {msg}");


        }

        private static void Server_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            Console.WriteLine($"Connected From {e.Client.RemoteEndPoint}");
        }
        private static void client_Disconnected(object sender, System.Net.Sockets.TcpClient e)
        {
            Console.WriteLine($"{e.Client.RemoteEndPoint} Disconnected");

        }

    }
}
