using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace Chatroom
{
    class Server
    {
        // Receive event triggered whenever a message is received
        public delegate void RxMessageEventHandler(string message);

        public event RxMessageEventHandler RxMessageEvent;

        protected virtual void OnRxMessageEvent(string message)
        {
            RxMessageEvent?.Invoke(message);
        }

        // Connection event triggered whenever a connection is established
        public delegate void CntEventHandler(long ID);

        public event CntEventHandler CntEvent;

        protected virtual void OnCntEvent(long ID)
        {
            CntEvent?.Invoke(ID);
        }

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

        public TcpListener ServerObj { get => server; set => server = value; }
        public int Port { get => port; set => port = value; }
        public IPAddress Ip { get => ip; set => ip = value; }
        public long ConnectionCount { get => connectionCount; set => connectionCount = value; }

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

        public void Start()
        {
            Console.WriteLine("Starting Server.");
            ConnectionCount = 0;
            server.Start();
            Rx.Start();
            Tx.Start();
            ConnectionMgr.Start();
            DisConnectionMgr.Start();
        }

        public void Close()
        {
            isExitting = true;
            while (transmitStatus || clientListEditStatus || readingStatus) ;
            foreach (NetUser user in users)
                user.Close();
            server.Stop();
        }

        public void TargetedTx(long id, string message)
        {
            // Sends a message to a specific user
            transmitStatus = true;
            while (clientListEditStatus) ;
            foreach(NetUser user in users)
            {
                if(id == user.Id)
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

        public void RxManager()
        {
            // Handles received messages
            Console.WriteLine("Receiver manager initialized, awaiting incoming messages...");
            while(!isExitting)
            {
                byte[] bytes = new byte[256];
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
                                OnRxMessageEvent( users[i].Username + ": " + msg);  // Triggers the receive event if a message was found
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
            while(!isExitting)
            {
                if(TxQueue.Count > 0)
                {
                    transmitStatus = true;
                    while (clientListEditStatus) ;  // Waits until it's safe to proceed

                    // Transmits a message if there exists one in the queue
                    byte[] msg = Encoding.ASCII.GetBytes(TxQueue.Dequeue());
                    
                    foreach(NetUser user in users)
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
            while(!isExitting)
            {
                TcpClient client = server.AcceptTcpClient();    // Waits until a connection appears

                await Task.Run(new Action(Login));

                // Helper function to keep everything fast
                void Login()
                {
                    //bool signup = false;
                    NetworkStream stream = client.GetStream();
                    byte[] responses = new byte[256];
                    int i = 0;
                    string username = "";
                    bool duplicant = false;
                    do
                    {
                        TargetedTx(stream, "Username:");
                        while ((i = stream.Read(responses, 0, responses.Length)) <= 0) ; // Wait until the person responds, or times out
                        username = Encoding.ASCII.GetString(responses);

                        while (clientListEditStatus) ;  // Waits until any pending removals are finished

                        clientListEditStatus = true;    // Flags that a change is occuring

                        while (readingStatus) ;         // Waits until the current read is finished

                        foreach (NetUser user in users)
                        {
                            if (username == user.Username)
                            {
                                TargetedTx(stream, "Sorry, that username is already taken.");
                                duplicant = true;
                                break;
                            }
                        }

                        clientListEditStatus = false;
                    }
                    while (duplicant);
                    /*
                    TargetedTx(stream, "Type NEW to sign up, otherwise, please type your credentials\nUsername:");
                    byte[] responses = new byte[256];
                    int i = 0;
                    // TODO: Implements a timeout function here.
                    while ((i = stream.Read(responses, 0, responses.Length)) <= 0) ; // Wait until the person responds, or times out
                    string username = Encoding.ASCII.GetString(responses);
                    if(username == "NEW")
                    {
                        signup = true;
                        bool duplicant = false;
                        do
                        {
                            TargetedTx(stream, "Username:");
                            while ((i = stream.Read(responses, 0, responses.Length)) <= 0) ; // Wait until the person responds, or times out
                            username = Encoding.ASCII.GetString(responses);

                            while (clientListEditStatus) ;  // Waits until any pending removals are finished

                            clientListEditStatus = true;    // Flags that a change is occuring

                            while (readingStatus) ;         // Waits until the current read is finished

                            foreach (NetUser user in users)
                            {
                                if (username == user.Username)
                                {
                                    TargetedTx(stream, "Sorry, that username is already taken.");
                                    duplicant = true;
                                    break;
                                }
                            }

                            clientListEditStatus = false;
                        }
                        while (duplicant);
                    }

                    /*
                    TargetedTx(stream, "Password:");
                    while ((i = stream.Read(responses, 0, responses.Length)) <= 0) ; // Wait until the person responds, or times out
                    string password = Encoding.ASCII.GetString(responses);
                    if(signup)
                    {
                        while (clientListEditStatus) ;  // Waits until any pending removals are finished

                        clientListEditStatus = true;    // Flags that a change is occuring

                        while (readingStatus) ;         // Waits until the current read is finished

                        // Adds the client to the list
                        users.Add(new NetUser(username, password, client));

                        OnCntEvent(users.Last().Id);

                        TxQueue.Enqueue(username + " has joined the room!");

                        clientListEditStatus = false;
                    }
                    else
                    {

                    }
                    */

                    while (clientListEditStatus) ;  // Waits until any pending removals are finished

                    clientListEditStatus = true;    // Flags that a change is occuring

                    while (readingStatus) ;         // Waits until the current read is finished

                    // Adds the client to the list
                    users.Add(new NetUser(username, "", client));

                    OnCntEvent(users.Last().Id);

                    TxQueue.Enqueue(username + " has joined the room!");
                    Console.WriteLine("{0} has joined the room!", username);

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
                if(ClosingQueue.Count > 0)
                {
                    int id = ClosingQueue.Dequeue();

                    while (clientListEditStatus) ;  // Waits until any pending new connections are finished

                    clientListEditStatus = true;

                    while (readingStatus||transmitStatus) ; // Waits until the current read/write is finished

                    // Closes and removes the client
                    for(int i = 0; i < users.Count; i++)
                    {
                        if(id == users[i].Id)
                        {
                            Console.WriteLine("{0} has left the room.", users[i].Username);
                            TxQueue.Enqueue(users[i].Username + " has left the room.");
                            users[i].Close();
                            users.RemoveAt(i);
                            break;
                        }
                    }

                    clientListEditStatus = false;
                }
            }
        }
    }
}
