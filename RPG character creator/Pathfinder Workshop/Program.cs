using Pathfinder_Workshop.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pathfinder_Workshop
{
    static class Program
    {
        // The main form's object
        private static Main_host mainForm;
        private static Queue<CommandSTYP> commandQueue = new Queue<CommandSTYP>();
        // Accessor Methods
        public static Main_host MainForm { get => mainForm; set => mainForm = value; }
        public static Queue<CommandSTYP> CommandQueue { get => commandQueue; set => commandQueue = value; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Main_host();
            Application.Run(mainForm);
        }
    }
}
