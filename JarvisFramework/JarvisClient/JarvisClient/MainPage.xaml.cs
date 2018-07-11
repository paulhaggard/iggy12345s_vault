using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using JarvisFramework;
using System.Net;

namespace JarvisClient
{
	public partial class MainPage : ContentPage
	{
        #region Properties

        private Label titleLabel;
        private Entry userEntryBar;
        private Label consoleOutput;
        private Client tcpClient;
        private bool isConnected = false;
        private bool isExitting = false;

        private Task consoleTask;
        private Queue<string> consoleQueue;

        #endregion

        #region Constructor

        public MainPage()
		{
			InitializeComponent();

            // Sets up the controls
            titleLabel = new Label { XAlign = TextAlignment.Center, Text = "Jarvis says hello!" };
            userEntryBar = new Entry { Placeholder = "Type a command..." };
            consoleOutput = new Label { XAlign = TextAlignment.Start, Text = "Hello World!\n" };

            // Binds events
            userEntryBar.Completed += UserEntryBar_Completed;

            // Sets up the console handler
            consoleQueue = new Queue<string>();
            consoleTask = new Task(ConsoleModule);
            consoleTask.Start();
            

            Content = new StackLayout
            {
                //BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                    {
                        titleLabel,
                        userEntryBar,
                        consoleOutput
                    }
            };

        }

        #endregion

        #region Accessor Methods

        public Label TitleLabel { get => titleLabel; set => titleLabel = value; }
        public Entry UserEntryBar { get => userEntryBar; set => userEntryBar = value; }
        public Label ConsoleOutput { get => consoleOutput; set => consoleOutput = value; }

        #endregion

        #region Methods

        /// <summary>
        /// Appends a string message to the console
        /// </summary>
        /// <param name="info">string to append to the console</param>
        private void AppendConsole(string info)
        {
            consoleQueue.Enqueue(info);
        }

        /// <summary>
        /// Connects the app to the tcp jarvis server
        /// </summary>
        /// <param name="port">port number of the server</param>
        /// <param name="ip">ip address of the server</param>
        /// <returns>Returns false if the app was already connected, returns true otherwise</returns>
        public bool ConnectTCP(int port, IPAddress ip)
        {
            if (isConnected)
                return false;
            tcpClient = new Client(port, ip);
            isConnected = true;
            return true;
        }

        #region Events

        private void UserEntryBar_Completed(object sender, EventArgs e)
        {
            switch (userEntryBar.Text)
            {
                case ("Exit"):
                    isExitting = true;
                    break;
                default:
                    AppendConsole(userEntryBar.Text);
                    userEntryBar.Text = "";
                    break;
            }
        }

        #endregion

        #endregion

        #region Modules

        /// <summary>
        /// Handles the console status, what appears on the console.
        /// </summary>
        private void ConsoleModule()
        {
            string previousOut = "";    // Used to check if the console has actually changed, prevents polling

            while(!isExitting)
            {
                if(consoleQueue.Count > 30)
                {
                    for(int i = 0; i < consoleQueue.Count; i++)
                    {
                        consoleQueue.Dequeue();
                    }
                }
                else if(consoleQueue.Count > 0 && consoleQueue.Peek() != previousOut)
                {
                    consoleOutput.Text = "";

                    foreach (string msg in consoleQueue)
                        consoleOutput.Text += msg + "\n";

                }
            }
        }

        #endregion
    }
}
