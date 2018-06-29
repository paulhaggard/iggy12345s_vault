using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;

namespace JarvisFramework
{
    public class NetUser
    {
        private string username;
        private string password;
        private TcpClient client;
        private NetworkStream stream;
        private long id;
        private static long userCount = 0;

        public string Username { get => username; }
        public TcpClient Client { get => client; set => client = value; }
        public NetworkStream Stream { get => stream; set => stream = value; }
        public long Id { get => id; set => id = value; }

        public NetUser(string username, string password, TcpClient client)
        {
            this.username = username;
            this.password = password;
            this.client = client;
            stream = client.GetStream();
            id = userCount++;
        }

        public void Close()
        {
            stream.Close();
            client.Close();
        }

        public bool CheckPassword(string password)
        {
            return this.password == password;
        }
    }
}
