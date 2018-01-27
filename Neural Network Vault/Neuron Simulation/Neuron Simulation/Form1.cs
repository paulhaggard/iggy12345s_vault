using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neuron_Simulation
{
    public partial class Form1 : Form
    {
        private ThreeBlue1BrownExample networkTest;

        public Form1()
        {
            InitializeComponent();
            statusStrip.Text = "Loading...";
            // Loads all of the memory we need to run this network
            networkTest = new ThreeBlue1BrownExample();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
