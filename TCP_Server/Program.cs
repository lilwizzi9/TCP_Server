using System;

using SimpleTCP;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

using System.Diagnostics;
using System.Collections.Generic;

namespace TCP_Server
{
    internal class Program
    {
        static Stopwatch sw;
        private static List<Player> players;

        private List<Objects.Actor> actors;
        static void Main(string[] args)
        {
            // init Player list
            players = new List<Player>{};
            // init connection
            System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse("0.0.0.0");
            //UdpClient udpServer = new UdpClient(5508);

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

            // Game Loop 
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
                    //Console.Clear();
                    //Console.WriteLine("Waiting For Connection...");
                }
                Thread.Sleep(3000);

                //foreach (Player i in players) {
                    //i.msg("Hello From C# Somone Just Joined");
                    
                //}

            }
        }

        private static void Server_DataReceived(object sender, Message e)
        {
            var msg = Encoding.UTF8.GetString(e.Data);
           // sw.Stop();
            //Console.Clear();
           // Console.WriteLine("Elapsed: " + sw.Elapsed);
            //sw.Restart();
            //Console.WriteLine($"{e.TcpClient.Client.RemoteEndPoint}: {msg}");
            foreach (Player p in players) {
                p.rcv(msg);
            }


        }

        

        private static void Server_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            Player NewPlayer = new Player($"{e.Client.RemoteEndPoint}", e,ref players);
            Console.WriteLine($"Connected From {e.Client.RemoteEndPoint}");
            // Send New Players Id To EveryOne and 
            // Send Evweyones Id To New Player
            foreach (Player p in players) {
                p.msg($"|SC:{NewPlayer.getId()}");
                NewPlayer.msg($"|SC:{p.getId()}");
            }
            // Add New player To Register
            players.Add(NewPlayer);
            foreach (Player p in players) {
                p.update_loc();
                foreach (Player v in players) {
                    p.update_loc(v.getId());
                }
            }
            



        }



        private static void client_Disconnected(object sender, System.Net.Sockets.TcpClient e)
        {
            Console.WriteLine($"{e.Client.RemoteEndPoint} Disconnected");
            for (int x = players.Count - 1; x > -1; x--)
            {

                if (players[x].getClient() == e)
                {
                    Player p = players[x];
                    players.Remove(players[x]);
                    foreach (Player i in players) { 
                        i.msg($"|DSPC:{p.getId()}");
                    }
                }
            }


        }
        private Objects.Actor spawn_actor(Player owner) {
            Objects.Actor actor = new Objects.Actor(owner, "","");

            

            actors.Add(actor);
            

            return (actor);
        }

        

    }
}
