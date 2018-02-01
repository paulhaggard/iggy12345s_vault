using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Neuron_Simulation.NeuralNetwork;

namespace Neuron_Simulation
{
    public partial class Form1 : Form
    {
        delegate void IncrementProgressBarCallback(double Error);

        public void OnTrainingUpdateEvent(object sender, TrainingUpdateEventArgs result)
        {
            // Executes every time the network finishes a training sample
            DrawNetwork(result.Layers);
            if(this.progressBar1.InvokeRequired)
            {
                IncrementProgressBarCallback d = new IncrementProgressBarCallback(IncrementProgressBar);
                this.Invoke(d, new object[] { result.Error });
            }
            else
                progressBar1.Increment(1);
        }

        public void IncrementProgressBar(double Error)
        {
            progressBar1.Increment(1);
            ErrorLabel.Text = Error.ToString();
        }

        private ThreeBlue1BrownExample networkTest;
        private List<List<Tuple<int, int>>> neuronCoord;
        private int plotSize;

        public Form1()
        {
            InitializeComponent();
            // Loads all of the memory we need to run this network
            // Generates the neural network
            networkTest = new ThreeBlue1BrownExample();
            networkTest.Net.GenWeightsAndBiases();
            networkTest.Net.TrainingUpdateEvent += OnTrainingUpdateEvent;

            // Stores a list of all of the coordinates of each neuron's position on the layout display.
            plotSize = 5;
            neuronCoord = new List<List<Tuple<int, int>>>(networkTest.Net.Layers.Count);
            GenNeuronCoord();

            // sets all the images up
            InputLayerActivations.Image = new Bitmap(InputLayerActivations.Width, InputLayerActivations.Height);
            InputLayerWeights.Image = new Bitmap(InputLayerWeights.Width, InputLayerWeights.Height);
            HiddenLayerAActivations.Image = new Bitmap(HiddenLayerAActivations.Width, HiddenLayerAActivations.Height);
            HiddenLayerBActivations.Image = new Bitmap(HiddenLayerBActivations.Width, HiddenLayerBActivations.Height);
            HiddenLayerAWeights.Image = new Bitmap(HiddenLayerAWeights.Width, HiddenLayerAWeights.Height);
            HiddenLayerBWeights.Image = new Bitmap(HiddenLayerBWeights.Width, HiddenLayerBWeights.Height);
            OutputLayerActivations.Image = new Bitmap(OutputLayerActivations.Width, OutputLayerActivations.Height);
            LayoutBox.Image = new Bitmap(LayoutBox.Width, LayoutBox.Height);

            // Draws the neural network onto the bitmaps and updates the activations and weight displays
            DrawNetwork(networkTest.Net.Layers);

            // Sets up the progress bar
            progressBar1.Maximum = networkTest.N_samples * networkTest.Iterations;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            progressBar1.Visible = false;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Starts the training
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            networkTest.Test();
        }

        private void GenNeuronCoord()
        {
            for (int i = 0; i < networkTest.Net.Layers.Count; i++)
            {
                List<Tuple<int, int>> temp = new List<Tuple<int, int>>(networkTest.Net.Layers[i].Count);
                double scale_x = (1 / (double)(networkTest.Net.Layers.Count)) * LayoutBox.Size.Width;
                double scale_y = (1 / (double)(networkTest.Net.Layers[i].Count)) * LayoutBox.Size.Height;
                double spacing = (LayoutBox.Size.Height - (networkTest.Net.Layers[i].Count - 1) * scale_y) / 2;
                for (int j = 0; j < networkTest.Net.Layers[i].Count; j++)
                {
                    int scaled_y = (int)(j * scale_y + spacing);
                    int scaled_x = (int)(i * scale_x);
                    temp.Add(new Tuple<int, int>(scaled_x, scaled_y));
                }
                neuronCoord.Add(temp);
            }
        }

        private void DrawNetwork(List<List<Neuron>> layers)
        {
            Image layout = LayoutBox.Image;
            Image inputAct = InputLayerActivations.Image;
            Image inputWt = InputLayerWeights.Image;
            Image layAAct = HiddenLayerAActivations.Image;
            Image layAWt = HiddenLayerAWeights.Image;
            Image layBAct = HiddenLayerBActivations.Image;
            Image layBWt = HiddenLayerBWeights.Image;
            Image outputAct = OutputLayerActivations.Image;

            NeuralNetwork net = networkTest.Net;

            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush, 1);
            

            using (Graphics g = Graphics.FromImage(layout))
            {
                g.Clear(Color.White);

                int i = 0;
                int j = 0;

                foreach(List<Neuron> layer in layers)
                {
                    j = 0;
                    foreach(Neuron neuron in layer)
                    {
                        g.DrawEllipse(pen, new Rectangle(new Point(neuronCoord[i][j].Item1, neuronCoord[i][j].Item2), new Size(plotSize, plotSize)));
                        if ((i > 0)&&(neuron.Weight_in.Count > 0))
                        {
                            for (int k = 0; k < neuronCoord[i - 1].Count; k++)
                            {
                                pen.Color = Color.FromArgb(255, (neuron.Weight_in[k] <= 0) ? 200 : 0, (Math.Abs(neuron.Weight_in[k]) > 1)?255:(int)(Math.Abs(neuron.Weight_in[k]) * 255), (neuron.Weight_in[k] > 0) ? 200 : 0);
                                g.DrawLine(pen, new Point(neuronCoord[i][j].Item1 + (plotSize / 2), neuronCoord[i][j].Item2 + (plotSize / 2)),
                                    new Point(neuronCoord[i-1][k].Item1 + (plotSize/2), neuronCoord[i-1][k].Item2 + (plotSize / 2)));
                            }
                        }
                        pen.Color = Color.Black;
                        j++;
                    }
                    i++;
                }
                LayoutBox.Invalidate();
            }

            using (Graphics g = Graphics.FromImage(inputAct))
            {
                g.Clear(Color.White);
                int i, j;
                i = 0;
                j = 0;
                int drawSize_x = (int)(InputLayerActivations.Size.Width / Math.Sqrt(layers[0].Count));
                int drawSize_y = (int)(InputLayerActivations.Size.Height / Math.Sqrt(layers[0].Count));
                foreach (Neuron neuron in layers[0])
                {
                    pen.Color = Color.FromArgb(255, (int)(neuron.Activation * 255), (int)(neuron.Activation * 200), (int)(neuron.Activation * 255));
                    g.FillRectangle(pen.Brush, new Rectangle(new Point(i * drawSize_x, j * drawSize_y), new Size(drawSize_x, drawSize_y)));
                    if(++i > Math.Sqrt(layers[0].Count)-1)
                    {
                        i = 0;
                        j++;
                    }
                }
            }

            using (Graphics g = Graphics.FromImage(inputWt))
            {
                g.Clear(Color.White);
                int i, j;
                i = 0;
                j = 0;
                int drawSize_x = (InputLayerWeights.Size.Width / layers[0].Count);
                if (drawSize_x == 0)
                    drawSize_x = 1;
                int drawSize_y = (InputLayerWeights.Size.Height / layers[1].Count);
                if (drawSize_y == 0)
                    drawSize_y = 1;
                foreach (Neuron neuron in layers[1])
                {
                    i = 0;
                    foreach (double weight in neuron.Weight_in)
                    {
                        pen.Color = Color.FromArgb(255, (weight <= 0) ? 200 : 0, (Math.Abs(weight) > 1) ? 255 : (int)(Math.Abs(weight) * 255), (weight > 0) ? 200 : 0);
                        g.FillRectangle(pen.Brush, new Rectangle(new Point(i++ * drawSize_x, j * drawSize_y), new Size(drawSize_x, drawSize_y)));
                    }
                    j++;
                }
                pen.Color = Color.Black;
            }

            using (Graphics g = Graphics.FromImage(layAAct))
            {
                g.Clear(Color.White);
                int i, j;
                i = 0;
                j = 0;
                int drawSize_x = (int)(HiddenLayerAActivations.Size.Width / Math.Sqrt(layers[1].Count));
                int drawSize_y = (int)(HiddenLayerAActivations.Size.Height / Math.Sqrt(layers[1].Count));
                foreach (Neuron neuron in layers[1])
                {
                    pen.Color = Color.FromArgb(255, (int)(neuron.Activation * 255), (int)(neuron.Activation * 200), (int)(neuron.Activation * 255));
                    g.FillRectangle(pen.Brush, new Rectangle(new Point(i * drawSize_x, j * drawSize_y), new Size(drawSize_x, drawSize_y)));
                    if (++i > Math.Sqrt(layers[1].Count) - 1)
                    {
                        i = 0;
                        j++;
                    }
                }
            }

            using (Graphics g = Graphics.FromImage(layAWt))
            {
                g.Clear(Color.White);
                int i, j;
                i = 0;
                j = 0;
                int drawSize_x = (InputLayerWeights.Size.Width / layers[1].Count);
                int drawSize_y = (InputLayerWeights.Size.Height / layers[2].Count);
                foreach (Neuron neuron in layers[2])
                {
                    i = 0;
                    foreach (double weight in neuron.Weight_in)
                    {
                        pen.Color = Color.FromArgb(255, (weight <= 0) ? 200 : 0, (Math.Abs(weight) > 1) ? 255 : (int)(Math.Abs(weight) * 255), (weight > 0) ? 200 : 0);
                        g.FillRectangle(pen.Brush, new Rectangle(new Point(i++ * drawSize_x, j * drawSize_y), new Size(drawSize_x, drawSize_y)));
                    }
                    j++;
                }
                pen.Color = Color.Black;
            }

            using (Graphics g = Graphics.FromImage(layBAct))
            {
                g.Clear(Color.White);
                int i, j;
                i = 0;
                j = 0;
                int drawSize_x = (int)(HiddenLayerBActivations.Size.Width / Math.Sqrt(layers[2].Count));
                int drawSize_y = (int)(HiddenLayerBActivations.Size.Height / Math.Sqrt(layers[2].Count));
                foreach (Neuron neuron in layers[2])
                {
                    pen.Color = Color.FromArgb(255, (int)(neuron.Activation * 255), (int)(neuron.Activation * 200), (int)(neuron.Activation * 255));
                    g.FillRectangle(pen.Brush, new Rectangle(new Point(i * drawSize_x, j * drawSize_y), new Size(drawSize_x, drawSize_y)));
                    if (++i > Math.Sqrt(layers[1].Count) - 1)
                    {
                        i = 0;
                        j++;
                    }
                }
            }

            using (Graphics g = Graphics.FromImage(layBWt))
            {
                g.Clear(Color.White);
                int i, j;
                i = 0;
                j = 0;
                int drawSize_x = (InputLayerWeights.Size.Width / layers[2].Count);
                int drawSize_y = (InputLayerWeights.Size.Height / layers[3].Count);
                foreach (Neuron neuron in layers[3])
                {
                    i = 0;
                    foreach (double weight in neuron.Weight_in)
                    {
                        pen.Color = Color.FromArgb(255, (weight <= 0) ? 200 : 0, (Math.Abs(weight) > 1) ? 255 : (int)(Math.Abs(weight) * 255), (weight > 0) ? 200 : 0);
                        g.FillRectangle(pen.Brush, new Rectangle(new Point(i++ * drawSize_x, j * drawSize_y), new Size(drawSize_x, drawSize_y)));
                    }
                    j++;
                }
                pen.Color = Color.Black;
            }

            using (Graphics g = Graphics.FromImage(outputAct))
            {
                g.Clear(Color.White);
                int i, j;
                i = 0;
                j = 0;
                int drawSize_x = (int)(HiddenLayerBActivations.Size.Width / Math.Sqrt(layers[1].Count));
                int drawSize_y = (int)(HiddenLayerBActivations.Size.Height / Math.Sqrt(layers[1].Count));
                foreach (Neuron neuron in layers[2])
                {
                    pen.Color = Color.FromArgb(255, (int)(neuron.Activation * 255), (int)(neuron.Activation * 200), (int)(neuron.Activation * 255));
                    g.FillRectangle(pen.Brush, new Rectangle(new Point(i * drawSize_x, j * drawSize_y), new Size(drawSize_x, drawSize_y)));
                    if (++i > Math.Sqrt(layers[1].Count - 1))
                    {
                        i = 0;
                        j++;
                    }
                }
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void TestNet_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Testing the network...");
            int a, b;
            a = 0;
            b = 0;
            for (int i = 0; i < 4; i++)
            {
                List<double> Result = networkTest.Net.Calc(new List<double> { a, b });
                Console.WriteLine("The result of inserting ({0},{1}) is: {2}", a, b, Result[0]);
                if(++a > 1)
                {
                    a = 0;
                    if (++b > 1)
                        b = 0;
                }
            }
        }
    }
}
