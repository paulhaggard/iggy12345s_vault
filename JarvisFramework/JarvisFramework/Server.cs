using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using JarvisFramework;

namespace JarvisFramework
{
    class Server
    {
        #region Events

        // Receive event triggered whenever a message is received
        public delegate void RxMessageEventHandler(object sender, ServerRxEventArgs e);

        /// <summary>
        /// Triggered anytime a message is received
        /// </summary>
        public event RxMessageEventHandler RxMessageEvent;

        protected virtual void OnRxMessageEvent(long ID, string message)
        {
            RxMessageEvent?.Invoke(this, new ServerRxEventArgs(ID, message));
        }

        // Receive event triggered whenever a message is received
        public delegate void TxMessageEventHandler(object sender, ServerTxEventArgs e);

        /// <summary>
        /// Triggered anytime a message is transmitted
        /// </summary>
        public event TxMessageEventHandler TxMessageEvent;

        protected virtual void OnTxMessageEvent(string message)
        {
            TxMessageEvent?.Invoke(this, new ServerTxEventArgs(message));
        }

        // Connection event triggered whenever a connection is established
        public delegate void CntEventHandler(long ID);

        /// <summary>
        /// Triggered anytime a connection is established
        /// </summary>
        public event CntEventHandler CntEvent;

        protected virtual void OnCntEvent(long ID)
        {
            CntEvent?.Invoke(ID);
        }

        #endregion

        #region Properties

        private TcpListener server;
        private int port;
        private IPAddress ip;
        private Thread Rx;
        private Thread Tx;
        private Thread ConnectionMgr;
        private Thread DisConnectionMgr;
        private bool isExitting;
        private Queue<string> TxQueue;
        private Queue<int> ClosingQueue;
        private bool clientListEditStatus;      // Flags when a channel list is being changed
        private bool readingStatus;             // Flags when the RxManager is iterating through the channel list
        private bool transmitStatus;            // Flags when the TxManager is trying to transmit a message
        private long connectionCount;           // How many connections have been made to the server
        private List<NetUser> users;

        #endregion

        #region Accessor Methods

        public TcpListener ServerObj { get => server; set => server = value; }
        public int Port { get => port; set => port = value; }
        public IPAddress Ip { get => ip; set => ip = value; }
        public long ConnectionCount { get => connectionCount; set => connectionCount = value; }

        #endregion

        #region Constructors

        public Server(int port, IPAddress ip)
        {
            this.port = port;
            this.ip = ip;
            server = new TcpListener(ip, port);
            users = new List<NetUser>();
            Rx = new Thread(new ThreadStart(RxManager));
            Tx = new Thread(new ThreadStart(TxManager));
            ConnectionMgr = new Thread(new ThreadStart(ConnectionManager));
            DisConnectionMgr = new Thread(new ThreadStart(DisConnectionManager));
            TxQueue = new Queue<string>();
            ClosingQueue = new Queue<int>();
            clientListEditStatus = false;
            readingStatus = false;
            transmitStatus = false;
        }

        public Server()
        {
            port = FindFreePort();
            ip = IPAddress.Loopback;
            server = new TcpListener(ip, port);
            users = new List<NetUser>();
            Rx = new Thread(new ThreadStart(RxManager));
            Tx = new Thread(new ThreadStart(TxManager));
            ConnectionMgr = new Thread(new ThreadStart(ConnectionManager));
            DisConnectionMgr = new Thread(new ThreadStart(DisConnectionManager));
            TxQueue = new Queue<string>();
            ClosingQueue = new Queue<int>();
            clientListEditStatus = false;
            readingStatus = false;
            transmitStatus = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Starting Server...");
            ConnectionCount = 0;
            server.Start();
            Rx.Start();
            Tx.Start();
            ConnectionMgr.Start();
            DisConnectionMgr.Start();
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Close()
        {
            isExitting = true;
            while (transmitStatus || clientListEditStatus || readingStatus) ;
            foreach (NetUser user in users)
                user.Close();
            server.Stop();
        }

        /// <summary>
        /// Finds the next available port
        /// </summary>
        /// <returns>Returns the next available port</returns>
        public int FindFreePort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        public void TargetedTx(long id, string message)
        {
            // Sends a message to a specific user
            transmitStatus = true;
            while (clientListEditStatus) ;
            foreach (NetUser user in users)
            {
                if (id == user.Id)
                {
                    byte[] msg = Encoding.ASCII.GetBytes(message);
                    user.Stream.Write(msg, 0, msg.Length);
                    break;
                }
            }
            transmitStatus = false;
        }

        public void TargetedTx(NetworkStream stream, string message)
        {
            // Sends a message to a specific user
            byte[] msg = Encoding.ASCII.GetBytes(message);
            stream.Write(msg, 0, msg.Length);
        }

        public void SendMessage(string message)
        {
            TxQueue.Enqueue(message);
        }

        #region Managers

        public void RxManager()
        {
            // Handles received messages
            Console.WriteLine("Receiver manager initialized, awaiting incoming messages...");
            while (!isExitting)
            {
                byte[] bytes = new byte[65536];
                // Checks if it can read
                if (!clientListEditStatus)
                {
                    readingStatus = true;
                    for (int i = 0; i < users.Count; i++)
                    {
                        int count = users[i].Stream.Read(bytes, 0, bytes.Length);
                        if (count > 0)
                        {
                            // Found a message waiting to be read
                            string msg = Encoding.ASCII.GetString(bytes, 0, count);
                            if (msg == "Connection.Close")
                                ClosingQueue.Enqueue(i);    // Closes the connection if requested.
                            else
                                OnRxMessageEvent(users[i].Id, msg);  // Triggers the receive event if a message was found
                        }
                    }
                    readingStatus = false;
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
                    transmitStatus = true;
                    while (clientListEditStatus) ;  // Waits until it's safe to proceed

                    string message = TxQueue.Dequeue();

                    OnTxMessageEvent(message);

                    // Transmits a message if there exists one in the queue
                    byte[] msg = Encoding.ASCII.GetBytes(message);

                    foreach (NetUser user in users)
                    {
                        // Transmits to every listener
                        user.Stream.Write(msg, 0, msg.Length);
                    }

                    transmitStatus = false;
                }
            }
        }

        public async void ConnectionManager()
        {
            // Handles new Connections
            Console.WriteLine("Connection Host Initialized, awaiting connections...");
            while (!isExitting)
            {
                TcpClient client = server.AcceptTcpClient();    // Waits until a connection appears

                await Task.Run(new Action(Login));

                // Helper function to keep everything fast
                void Login()
                {
                    //bool signup = false;
                    NetworkStream stream = client.GetStream();
                    byte[] responses = new byte[65536];
                    int i = 0;
                    string username = "";
                    string password = "";
                    bool duplicant = false;
                    do
                    {
                        duplicant = false;
                        TargetedTx(stream, "Username: ");
                        while ((i = stream.Read(responses, 0, responses.Length)) <= 0) ; // Wait until the person responds, or times out

                        username = Encoding.ASCII.GetString(responses, 0, i);

                        TargetedTx(stream, "Password: ");
                        while ((i = stream.Read(responses, 0, responses.Length)) <= 0) ; // Wait until the person responds, or times out

                        password = Encoding.ASCII.GetString(responses, 0, i);

                        while (clientListEditStatus) ;  // Waits until any pending removals are finished

                        clientListEditStatus = true;    // Flags that a change is occuring

                        while (readingStatus) ;         // Waits until the current read is finished

                        foreach (NetUser user in users)
                        {
                            if (username == user.Username)
                            {
                                if (user.CheckPassword(password))
                                    break;
                                TargetedTx(stream, "Username or password is incorrect.");
                                duplicant = true;
                            }
                        }

                        clientListEditStatus = false;
                    }
                    while (duplicant);
                    
                    while (clientListEditStatus) ;  // Waits until any pending removals are finished

                    clientListEditStatus = true;    // Flags that a change is occuring

                    while (readingStatus) ;         // Waits until the current read is finished

                    // Adds the client to the list
                    //users.Add(new NetUser(username, "", client));

                    OnCntEvent(users.Last().Id);

                    Console.WriteLine("{0} has Connected!", username);

                    clientListEditStatus = false;
                }
            }
        }

        public void DisConnectionManager()
        {
            // Handles closing Connections
            Console.WriteLine("Connection Host Initialized, awaiting connections...");
            while (!isExitting)
            {
                if (ClosingQueue.Count > 0)
                {
                    int id = ClosingQueue.Dequeue();

                    while (clientListEditStatus) ;  // Waits until any pending new connections are finished

                    clientListEditStatus = true;

                    while (readingStatus || transmitStatus) ; // Waits until the current read/write is finished

                    // Closes and removes the client
                    for (int i = 0; i < users.Count; i++)
                    {
                        if (id == users[i].Id)
                        {
                            Console.WriteLine("{0} has disconnected.", users[i].Username);
                            users[i].Close();
                            break;
                        }
                    }

                    clientListEditStatus = false;
                }
            }
        }

        #endregion

        #endregion
    }
}
