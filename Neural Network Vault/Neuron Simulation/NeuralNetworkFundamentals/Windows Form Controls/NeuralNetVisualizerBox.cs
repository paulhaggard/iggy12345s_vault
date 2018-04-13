using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetworkFundamentals.Windows_Form_Controls
{
    public partial class NeuralNetVisualizerBox : UserControl
    {
        // Properties
        private NeuralNetwork net;
        private List<List<Tuple<int, int>>> neuronCoord;
        private NeuralNetwork netRef;
        private int plotSize;

        // Accessor Methods
        public NeuralNetwork Net { get => net; set => SetupNewNetwork(value); }
        public int PlotSize { get => plotSize; set => plotSize = value; }

        public NeuralNetVisualizerBox(ref NeuralNetwork net, int plotSize = 5)
        {
            InitializeComponent();
            SetupNewNetwork(net);
            this.plotSize = plotSize;

            // Adjusts the picturebox control.
            picturebox.Width = Width;
            picturebox.Height = Height;
        }

        public void TrainingUpdateEvent(object sender, TrainingUpdateEventArgs e)
        {
            net.Layers = e.Layers;
            PaintNetwork(net);  // Calls the painter.
        }

        private void SetupNewNetwork(NeuralNetwork net)
        {
            // Updates the subscription status of the events
            netRef.TrainingUpdateEvent -= TrainingUpdateEvent;
            netRef = net;
            net.TrainingUpdateEvent += TrainingUpdateEvent;

            // Clones the new network and regenerates the coordinate list
            this.net = NeuralNetwork.Clone(net);
            neuronCoord = new List<List<Tuple<int, int>>>(this.net.Layers.Count);
            GenNeuronCoord();

            // Sets up the bitmap
            picturebox.Image = new Bitmap(picturebox.Width, picturebox.Height);
        }

        protected virtual void PaintNetwork(NeuralNetwork net)
        {
            // Paints the network onto the image
            NeuralNetwork netTemp = NeuralNetwork.Clone(net);   // Creates a clone of the network to prevent simultaneaous accessing.

            // Sets up the canvas
            Image picture = picturebox.Image;
            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush, 1);

            using (Graphics g = Graphics.FromImage(picture))
            {
                g.Clear(Color.DimGray);

                int i = 0;
                int j = 0;

                foreach (List<Neuron> layer in netTemp.Layers)
                {
                    j = 0;
                    foreach (Neuron neuron in layer)
                    {
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
                                g.DrawLine(pen, new Point(neuronCoord[i][j].Item1 + (PlotSize / 2), neuronCoord[i][j].Item2 + (PlotSize / 2)),
                                    new Point(neuronCoord[i - 1][k].Item1 + (PlotSize / 2), neuronCoord[i - 1][k].Item2 + (PlotSize / 2)));
                            }
                        }
                        pen.Color = Color.Black;
                        j++;
                    }
                    i++;
                }
                picturebox.Invalidate();
            }
        }

        protected virtual void GenNeuronCoord()
        {
            // Generates the coordinate locations for each neuron in the network.
            for (int i = 0; i < net.Layers.Count; i++)
            {
                List<Tuple<int, int>> temp = new List<Tuple<int, int>>(net.Layers[i].Count);
                double scale_x = (1 / (double)(net.Layers.Count)) * picturebox.Size.Width;
                double scale_y = (1 / (double)(net.Layers[i].Count)) * picturebox.Size.Height;
                double spacing = (picturebox.Size.Height - (net.Layers[i].Count - 1) * scale_y) / 2;
                for (int j = 0; j < net.Layers[i].Count; j++)
                {
                    int scaled_y = (int)(j * scale_y + spacing);
                    int scaled_x = (int)(i * scale_x);
                    temp.Add(new Tuple<int, int>(scaled_x, scaled_y));
                }
                neuronCoord.Add(temp);
            }
        }

        private void NeuralNetVisualizerBox_Resize(object sender, EventArgs e)
        {
            picturebox.Width = Width;
            picturebox.Height = Height;
        }
    }
}
