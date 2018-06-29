using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisFramework
{
    public class ServerRxEventArgs : EventArgs
    {
        private string message;
        private long id;

        public ServerRxEventArgs(long ID, string message = "")
        {
            this.message = message;
            id = ID;
        }

        public string Message { get => message; set => message = value; }
        public long ID { get => id; set => id = value; }
    }

    public class ServerTxEventArgs : EventArgs
    {
        private string message;

        public ServerTxEventArgs(string message = "")
        {
            this.message = message;
        }

        public string Message { get => message; set => message = value; }
    }
}
