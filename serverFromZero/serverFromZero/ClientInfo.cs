using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverFromZero
{
    [Serializable]
    public class ClientInfo
    {
        public string UserName;
        public string Password;
        [NonSerialized] public bool LoggedIn;      // Is logged in and connected?
        [NonSerialized] public Client Connection;  // Connection info
        
        public ClientInfo(string user, string pass)
        {
            this.UserName = user;
            this.Password = pass;
            this.LoggedIn = false;
        }
        public ClientInfo(string user, string pass, Client conn)
        {
            this.UserName = user;
            this.Password = pass;
            this.LoggedIn = true;
            this.Connection = conn;
        }
    }
}
