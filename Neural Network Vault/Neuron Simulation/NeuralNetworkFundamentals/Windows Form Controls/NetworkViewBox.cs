using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetworkFundamentals.Windows_Form_Controls
{
    public partial class NetworkViewBox : Form
    {
        private NeuralNetwork net;
        private List<List<Tuple<int, int>>> neuronCoord;
        private int plotSize;

        // Accessor Methods
        public NeuralNetwork Net { get => net; set => SetupNewNetwork(value); }
        public int PlotSize { get => plotSize; set => plotSize = value; }

        public NetworkViewBox()
        {
            InitializeComponent();
            plotSize = 5;
            Setup();
        }

        public NetworkViewBox(ref NeuralNetwork net, int plotSize = 5)
        {
            InitializeComponent();
            this.net = net;
            this.plotSize = plotSize;
            Setup();
        }

        protected virtual void Setup()
        {
            pictureBox1.Image = new Bitmap(Width, Height);
        }

        private void SetupNewNetwork(NeuralNetwork NewNet)
        {
            if (NewNet != null)
            {
                // Updates the subscription status of the events
                if (net != null)
                    net.TrainingUpdateEvent -= TrainingUpdateEvent;
                net = NewNet;
                NewNet.TrainingUpdateEvent += TrainingUpdateEvent;

                // Clones the new network and regenerates the coordinate list
                net = NeuralNetwork.Clone(NewNet);
                neuronCoord = new List<List<Tuple<int, int>>>(net.Layers.Count);
                GenNeuronCoord();

                // Sets up the bitmap
                pictureBox1.Image = new Bitmap(Width, Height);

            }
        }

        public void TrainingUpdateEvent(object sender, TrainingUpdateEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                PaintNetworkCallback d = PaintNetwork;
                Task.Factory.StartNew(() => BeginInvoke(d, new object[] { net.Layers }));
            });  // Calls the painter on a different thread.
        }

        protected delegate void PaintNetworkCallback(List<List<Neuron>> layers);

        public virtual void PaintNetwork(List<List<Neuron>> layers)
        {
            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen((Brush)brush.Clone(), 1);

            lock (pictureBox1.Image)
            {
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.Clear(Color.DimGray);

                    int i = 0;
                    int j = 0;

                    foreach (List<Neuron> layer in layers)
                    {
                        j = 0;
                        foreach (Neuron neuron in layer)
                        {
                            g.DrawEllipse(pen, new Rectangle(new Point(neuronCoord[i][j].Item1, neuronCoord[i][j].Item2), new Size(plotSize, plotSize)));
                            if ((i > 0) && (neuron.PrevWeights.Count > 0))
                            {
                                for (int k = 0; k < neuronCoord[i - 1].Count; k++)
                                {
                                    int blue = (neuron.Weights[0] <= neuron.PrevWeights[0]) ? 255 : 0; //(int)(Math.Abs(1 - (neuron.PrevDelta - neuron.PrevDelta)) * 255);
                                    int red = (neuron.Weights[0] > neuron.PrevWeights[0]) ? 255 : 0;//(int)(Math.Abs(1 - (neuron.PrevWeights[0] - neuron.Weights[0])) * 255);
                                    int green = (int)(Math.Abs(neuron.Activation * 127));
                                    pen.Color = Color.FromArgb(255,
                                        (red > 255) ? 255 : ((red < 0) ? 0 : red),
                                        (green > 127) ? 127 : ((green < 0) ? 0 : green),
                                        (blue > 255) ? 255 : ((blue < 0) ? 0 : blue));
                                    g.DrawLine(pen, new Point(neuronCoord[i][j].Item1 + (plotSize / 2), neuronCoord[i][j].Item2 + (plotSize / 2)),
                                        new Point(neuronCoord[i - 1][k].Item1 + (plotSize / 2), neuronCoord[i - 1][k].Item2 + (plotSize / 2)));
                                }
                            }
                            pen.Color = Color.Black;
                            j++;
                        }
                        i++;
                    }
                    pictureBox1.Invalidate();
                }
            }
        }

        protected virtual void GenNeuronCoord()
        {
            // Generates the coordinate locations for each neuron in the network.
            for (int i = 0; i < net.Layers.Count; i++)
            {
                List<Tuple<int, int>> temp = new List<Tuple<int, int>>(net.Layers[i].Count);
                double scale_x = (1 / (double)(net.Layers.Count)) * Size.Width;
                double scale_y = (1 / (double)(net.Layers[i].Count)) * Size.Height;
                double spacing = (Size.Height - (net.Layers[i].Count - 1) * scale_y) / 2;
                for (int j = 0; j < net.Layers[i].Count; j++)
                {
                    int scaled_y = (int)(j * scale_y + spacing);
                    int scaled_x = (int)(i * scale_x);
                    temp.Add(new Tuple<int, int>(scaled_x, scaled_y));
                }
                neuronCoord.Add(temp);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (net != null)
            {
                Task.Factory.StartNew(() =>
                {
                    PaintNetworkCallback d = PaintNetwork;
                    Task.Factory.StartNew(() => BeginInvoke(d, new object[] { net.Layers }));
                });  // Calls the painter on a different thread.
            }
        }
    }
}
