using System;
using System.Net;
using System.Text;

namespace Chatroom
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the chat room!\n");
            bool isSelected = false;
            while (!isSelected)
            {
                Console.WriteLine("What would you like to do?\n1) Host a server\n2) Connect to a server\n\nYour selection: ");
                switch(Console.ReadKey().KeyChar)
                {
                    case '1':
                        Host();
                        isSelected = true;
                        break;

                    case '2':
                        Connect();
                        isSelected = true;
                        break;

                    default:
                        Console.WriteLine("Invalid Selection, please try again...\n");
                        break;
                }
            }

            void Host()
            {
                Console.WriteLine("\nPlease specify a server ip address for the room: ");
                IPAddress ip = IPAddress.Parse(Console.ReadLine());
                Console.WriteLine("\nPlease specify the port to use: ");
                int port = Convert.ToInt32(Console.ReadLine());
                Server server = new Server(port, ip);
                server.RxMessageEvent += OnRx;
                server.Start();
                Console.Clear();
                Console.WriteLine("Welcome!");
                string entry = "";
                while(entry != "$Exit")
                {
                    entry = Console.ReadLine();
                    server.SendMessage(entry);
                }
                server.Close();
            }

            void OnRx(string message)
            {
                Console.WriteLine(message);
            }

            void Connect()
            {
                Console.WriteLine("\nPlease specify a server ip address for the room: ");
                IPAddress ip = IPAddress.Parse(Console.ReadLine());
                Console.WriteLine("\nPlease specify the port to use: ");
                int port = Convert.ToInt32(Console.ReadLine());
                Client server = new Client(port, ip);
                server.RxMessageEvent += OnRx;
                server.Start();
                Console.Clear();
                Console.WriteLine("Welcome!");
                string entry = "";
                while (entry != "$Exit")
                {
                    entry = Console.ReadLine();
                    server.SendMessage(entry);
                }
                server.Close();
            }

        }
    }
}
