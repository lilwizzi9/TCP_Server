using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCP_Server
{
    
    internal class Player
    {

        // input
        private double stickinput;
        // record player Start Loc to use 
        private string location = "(X=-69.939301,Y=-1628.697021,Z=457.307983)";
        private string last_location;
        private string control_rot;
        private string input ="0.0,0.0";

        // Data
        private string id;
        private string name;
        private int index;

        // connection
        private System.Net.Sockets.TcpClient conn;
        private List<Player> players;



        public Player(string Name, System.Net.Sockets.TcpClient conn, ref List<Player> players)
        {
            this.name = Name;
            this.id = get_rand_ID(5);
            this.conn = conn;
            this.players = players;

            bool updated = false;

            while (!updated)
            {
                if (msg($"|SUID:{this.id}")) {
                    Thread.Sleep(500);
                    msg($"{this.id}:Rep_loc:{location}");
                    updated = true;
                }

            }
        }

        public void set_index(int index) { 
            this.index = index;
        
        }

        

        public static string get_rand_ID(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool msg(string s)
        {
            if (s == null) return false;

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(s);

            
            try {
                // Get a client stream for reading and writing. 
                NetworkStream stream = conn.GetStream();
                // Send the message to the connected Player. 
                stream.Write(data, 0, data.Length);
            } catch (Exception e) {
                Console.WriteLine($"Error: {e}||");
                return false;
            }
            return true;
        }

        

        public System.Net.Sockets.TcpClient getClient() { 
            return conn;
        }
        public string getId() { return id; }

        public void update_loc(string uid =">") {
            if (uid == ">")
            {
                msg($"{this.id}:player_loc:{location}");
            }
            else {
                msg($"{uid}:player_loc:{location}");
            }

        }

        public void multicast(String s, bool self = true) {
            foreach (Player p in players) {
                if (self)
                {
                    p.msg(s);
                }
                else {
                    if (p != this) {
                        p.msg(s);
                    }
                }
                
            }
        }

        public void rcv(string data ) {
                string in_id = data.Split(":")[0];
                string _event = data.Split(":")[1];
                string info = data.Split(":")[2];
            if (data.StartsWith(this.id)) {
                // data for me
                location = info;
                if (_event == "Rep_loc")
                {
                    foreach (Player p in players) {

                        Console.WriteLine(info);
                        p.msg($"{this.id}:Rep_loc:{location}");

                    }

                } else if (_event == "input") {
                   Console.WriteLine("axis" + info);
                   // movment prediction based on input
                    input = info;
                    multicast($"{this.id}:input:{info}",false);
                } else if(_event == "rot") { 
                    // Character Control Rotation
                    Console.WriteLine("Rot" + info);
                    multicast($"{this.id}:rot:{info}");

                }


            }
        }


    }
}
