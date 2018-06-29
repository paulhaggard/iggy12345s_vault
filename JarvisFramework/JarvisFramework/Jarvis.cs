using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisFramework
{
    public class Jarvis
    {
        #region Properties

        private Server server;

        #endregion

        #region Constructors

        public Jarvis()
        {
            server = new Server();
            server.RxMessageEvent += OnRxMessage;
            ServerStart();
        }

        #endregion

        #region Methods

        public void ServerStart()
        {
            server.Start();
            Console.WriteLine("Created the server @{0}:{1}", server.Ip, server.Port);
        }

        #region Event Methods

        public void OnRxMessage(object sender, ServerRxEventArgs e)
        {
            Console.WriteLine("Received a message from {0}: {1}", e.ID, e.Message);
        }

        public void OnCnt(long ID)
        {
            Console.WriteLine("User {0} connected!", ID);
        }

        #endregion

        #endregion
    }
}
