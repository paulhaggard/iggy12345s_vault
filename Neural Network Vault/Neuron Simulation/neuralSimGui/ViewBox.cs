using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeuralNetworkFundamentals;

namespace neuralSimGui
{
    public partial class ViewBox : Form
    {
        private NeuralNetwork net;

        public ViewBox(ref NeuralNetwork net)
        {
            InitializeComponent();
            this.net = net;
            networkViewer1 = new NeuralNetworkFundamentals.Windows_Form_Controls.NetworkViewer(net);

        }

        public void UpdateVisual()
        {
            networkViewer1.PaintNetwork(net);
        }
    }
}
