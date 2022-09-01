using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCP_Server.Objects
{
    
    internal class Actor
    {
        public string location;
        public string rotation;
        public string id;
        public string name;
        public Player owner;

        public Actor(Player owner,string loc,string rot) {
            id = get_rand_ID(5);
        }

        public static string get_rand_ID(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789&%%##@#$%^&*(()_+=-}{:;";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
    }
}
