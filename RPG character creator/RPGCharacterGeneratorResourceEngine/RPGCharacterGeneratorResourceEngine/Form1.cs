using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreadHelper_Library;

namespace RPGCharacterGeneratorResourceEngine
{
    public partial class Form1 : Form
    {
        private ThreadLauncher<CommQueueDefaultData> threadManager;

        public Form1()
        {
            InitializeComponent();
            threadManager = new ThreadLauncher<CommQueueDefaultData>();
            List<TSubject<CommQueueDefaultData>> modules = new List<TSubject<CommQueueDefaultData>>();
            // TODO: Add background modules and forms here

            threadManager.Add(modules);
            threadManager.LaunchAll();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Closes the program
            Dispose();
        }
    }
}
