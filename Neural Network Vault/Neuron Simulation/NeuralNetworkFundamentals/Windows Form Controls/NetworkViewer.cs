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
    public partial class NetworkViewer : PictureBox
    {
        // Properties
        private NeuralNetwork net;
        private List<List<Tuple<int, int>>> neuronCoord;
        private NeuralNetwork netRef;
        private int plotSize;
        private Graphics g;

        // Accessor Methods
        public NeuralNetwork Net { get => net; set => SetupNewNetwork(value); }
        public int PlotSize { get => plotSize; set => plotSize = value; }

        public NetworkViewer(ref NeuralNetwork net, int plotSize = 5) : base()
        {
            InitializeComponent();
            SetupNewNetwork(net??new NeuralNetwork());
            this.plotSize = plotSize;
            
        }

        // Only for the designer
        public NetworkViewer() : base()
        {
            InitializeComponent();
            Image = new Bitmap(Width, Height);
        }

        public void TrainingUpdateEvent(object sender, TrainingUpdateEventArgs e)
        {
            // If the window hasn't been initialized, then initialize it, so the drawing shows up.
            if (!IsHandleCreated)
                CreateControl();

            Task.Factory.StartNew(() =>
            {
                PaintNetworkCallback d = PaintNetwork;
                Task.Factory.StartNew(() => BeginInvoke(d, new object[] { net.Layers }));
            });  // Calls the painter on a different thread.
        }

        private void SetupNewNetwork(NeuralNetwork NewNet)
        {
            if (NewNet != null)
            {
                // Updates the subscription status of the events
                if (netRef != null)
                    netRef.TrainingUpdateEvent -= TrainingUpdateEvent;
                netRef = NewNet;
                NewNet.TrainingUpdateEvent += TrainingUpdateEvent;

                // Clones the new network and regenerates the coordinate list
                net = NeuralNetwork.Clone(NewNet);
                neuronCoord = new List<List<Tuple<int, int>>>(net.Layers.Count);
                GenNeuronCoord();

                // Sets up the bitmap
                Image = new Bitmap(Width, Height);

            }
        }

        protected delegate void PaintNetworkCallback(List<List<Neuron>> layers);

        public virtual void PaintNetwork(List<List<Neuron>> layers)
        {
            // If the window hasn't been initialized, then initialize it, so the drawing shows up.
            if (!IsHandleCreated)
                CreateControl();

            // If the image can't be seen, then make it visible.
            if (!Visible)
                Visible = true;

            // Paints the network onto the image
            lock (Image)
            {
                g = Graphics.FromImage((Image)Image.Clone());
                Brush brush = new SolidBrush(Color.Black);
                Pen pen = new Pen((Brush)brush.Clone(), 1);

                using (g)
                {
                    g.Clear(Color.DimGray);

                    int i = 0;
                    int j = 0;

                    foreach (List<Neuron> layer in layers)
                    {
                        j = 0;
                        foreach (Neuron neuron in layer)
                        {
                            lock(pen)
                                g.DrawEllipse(pen, new Rectangle(new Point(neuronCoord[i][j].Item1, neuronCoord[i][j].Item2), new Size(PlotSize, PlotSize)));
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
                                    lock(pen)
                                        g.DrawLine(pen, new Point(neuronCoord[i][j].Item1 + (PlotSize / 2), neuronCoord[i][j].Item2 + (PlotSize / 2)),
                                            new Point(neuronCoord[i - 1][k].Item1 + (PlotSize / 2), neuronCoord[i - 1][k].Item2 + (PlotSize / 2)));
                                }
                            }
                            pen.Color = Color.Black;
                            j++;
                        }
                        i++;
                    }
                    Invalidate();
                    //g.Dispose();
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

        protected override void OnPaint(PaintEventArgs pe)
        { 
            base.OnPaint(pe);
        }
    }
}
