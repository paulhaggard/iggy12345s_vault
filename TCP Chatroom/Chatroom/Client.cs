using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Chatroom
{
    class Client
    {
        // Receive event triggered whenever a message is received
        public delegate void RxMessageEventHandler(string message);

        public event RxMessageEventHandler RxMessageEvent;

        protected virtual void OnRxMessageEvent(string message)
        {
            RxMessageEvent?.Invoke(message);
        }

        private TcpClient server;
        private int port;
        private IPAddress ip;
        private Action Rx;
        private Action Tx;
        private NetworkStream stream;
        private bool isExitting;
        private Queue<string> TxQueue;

        public TcpClient ServerObj { get => server; set => server = value; }
        public int Port { get => port; set => port = value; }
        public IPAddress Ip { get => ip; set => ip = value; }

        public Client(int port, IPAddress ip)
        {
            this.port = port;
            this.ip = ip;
            server = new TcpClient();
            Rx = new Action(RxManager);
            Tx = new Action(TxManager);
            TxQueue = new Queue<string>();
        }

        public async void Start()
        {
            Console.WriteLine("Attempting to connect to the host.");
            server.Connect(ip, port);
            stream = server.GetStream();
            await Task.Run(Rx);
            await Task.Run(Tx);
        }

        public void Close()
        {
            isExitting = true;
            byte[] msg = Encoding.ASCII.GetBytes("Connection.Close");
            stream.Write(msg, 0, msg.Length);
            stream.Close();
            server.Close();
        }

        public void RxManager()
        {
            // Handles received messages
            Console.WriteLine("Receiver manager initialized, awaiting incoming messages...");
            while (!isExitting)
            {
                byte[] bytes = new byte[256];
                // Checks if it can read
                int count = stream.Read(bytes, 0, bytes.Length);
                if (count > 0)
                {
                    // Found a message waiting to be read
                    string msg = Encoding.ASCII.GetString(bytes, 0, count);
                    OnRxMessageEvent(msg);  // Triggers the receive event if a message was found
                }
            }
        }

        public void TxManager()
        {
            // Handles Transmitted messges
            Console.WriteLine("Transmission manager initialized, awaiting outgoing messages...");
            while (!isExitting)
            {
                if (TxQueue.Count > 0)
                {
                    // Transmits a message if there exists one in the queue
                    byte[] msg = Encoding.ASCII.GetBytes(TxQueue.Dequeue());
                    // Transmits to the host
                    stream.Write(msg, 0, msg.Length);
                }
            }
        }

        public void SendMessage(string message)
        {
            TxQueue.Enqueue(message);
        }
    }
}
