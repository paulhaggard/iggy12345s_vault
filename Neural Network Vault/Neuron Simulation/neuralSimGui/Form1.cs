using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NeuralNetworkFundamentals;

namespace neuralSimGui
{
    public partial class Form1 : Form
    {
        delegate void IncrementProgressBarCallback(double Error);

        public void OnTrainingUpdateEvent(object sender, TrainingUpdateEventArgs result)
        {
            // Executes every time the network finishes a training sample
            Console.WriteLine("{0} XOR {1} is {2}", result.Layers[0][0].RawInput, result.Layers[0][1].RawInput, result.Layers.Last()[0].Activation);
            DrawingQueue.Enqueue(result.Layers);    // Adds this layer set to the queue to be drawn
            if (progressBar1.InvokeRequired)
            {
                IncrementProgressBarCallback d = new IncrementProgressBarCallback(IncrementProgressBar);
                Invoke(d, new object[] { result.Error });
            }
            else
                progressBar1.Increment(1);
        }

        public void IncrementProgressBar(double Error)
        {
            progressBar1.Increment(1);
            ErrorLabel.Text = Error.ToString();
        }

        private NeuralNetwork networkTest;
        private List<List<Tuple<int, int>>> neuronCoord;
        private int plotSize;
        private List<List<List<double>>> prevWeights;
        private Thread DrawingControllerThread;
        private Queue<List<List<Neuron>>> DrawingQueue;
        private bool IsExitting;
        private List<List<double>> inputSamp = new List<List<double>>();
        private List<List<double>> outputSamp = new List<List<double>>();
        Random Rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            // Loads all of the memory we need to run this network
            IsExitting = false;
            // Generates the neural network
            List<int> numOfNeurons = new List<int> { 2, 3, 3, 2 };  // Layer info
            networkTest = new NeuralNetwork(numOfNeurons);
            networkTest.GenWeightsAndBiases();
            networkTest.TrainingUpdateEvent += OnTrainingUpdateEvent;

            ResetNetworkCheckbox.Checked = true;

            // Populates the weight and prevWeight lists
            prevWeights = new List<List<List<double>>>(networkTest.Layers.Count);
            for (int i = 0; i < networkTest.Layers.Count; i++)
            {
                prevWeights.Add(new List<List<double>>(networkTest.Layers[i].Count));
                for (int j = 0; j < networkTest.Layers[i].Count; j++)
                {
                    prevWeights[i].Add(networkTest.Layers[i][j].Weights);
                }
            }

            // Stores a list of all of the coordinates of each neuron's position on the layout display.
            plotSize = 5;
            neuronCoord = new List<List<Tuple<int, int>>>(networkTest.Layers.Count);
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

            // Sets up the thread that will control graphics
            DrawingQueue = new Queue<List<List<Neuron>>>();
            DrawingControllerThread = new Thread(new ThreadStart(DrawingController));
            DrawingControllerThread.Start();

            // Draws the neural network onto the bitmaps and updates the activations and weight displays
            DrawingQueue.Enqueue(networkTest.Layers);

            // Sets up the progress bar
            progressBar1.Maximum =(int) (numSampCtrl.Value * numItrCtrl.Value);
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            progressBar1.Visible = false;

            //xor gate tester
            inputSamp.Add(new List<double>() { 0, 0 });
            inputSamp.Add(new List<double>() { 1, 0 });
            inputSamp.Add(new List<double>() { 0, 1 });
            inputSamp.Add(new List<double>() { 1, 1 });


            outputSamp.Add(new List<double>() { 0, 1 });
            outputSamp.Add(new List<double>() { 1, 0 });
            outputSamp.Add(new List<double>() { 1, 0 });
            outputSamp.Add(new List<double>() { 0, 1 });

            //sets up the samples given to the network
            //List<List<double>> inputSamp = new List<List<double>>();
            //inputSamp.Add(new List<double>());
            /*
            for(int i= 0; i<((int)(numSampCtrl.Value)); i++)
            {
                inputSamp.Add(new List<double>());
                for (int j = 0; j < ((int)(numSampCtrl.Value)); j++)
                {
                    inputSamp[i].Add(Rnd.Next());
                }
            }
            //List<List<double>> outputSamp = new List<List<double>>();
            //outputSamp.Add(new List<double>());
            for (int i = 0; i < ((int)(numSampCtrl.Value)); i++)
            {
                outputSamp.Add(new List<double>());
                for (int j = 0; j < ((int)(numSampCtrl.Value)); j++)
                {
                    outputSamp[i].Add(Rnd.Next());
                }
            }
            */

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            IsExitting = true;
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Starts the training
            progressBar1.Maximum = (int)(inputSamp.Count * numItrCtrl.Value); // Sets up the progress bar
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            networkTest.Train(((int)(numItrCtrl.Value)), inputSamp, outputSamp, Reset: ResetNetworkCheckbox.Checked);
        }

        private void GenNeuronCoord()
        {
            for (int i = 0; i < networkTest.Layers.Count; i++)
            {
                List<Tuple<int, int>> temp = new List<Tuple<int, int>>(networkTest.Layers[i].Count);
                double scale_x = (1 / (double)(networkTest.Layers.Count)) * LayoutBox.Size.Width;
                double scale_y = (1 / (double)(networkTest.Layers[i].Count)) * LayoutBox.Size.Height;
                double spacing = (LayoutBox.Size.Height - (networkTest.Layers[i].Count - 1) * scale_y) / 2;
                for (int j = 0; j < networkTest.Layers[i].Count; j++)
                {
                    int scaled_y = (int)(j * scale_y + spacing);
                    int scaled_x = (int)(i * scale_x);
                    temp.Add(new Tuple<int, int>(scaled_x, scaled_y));
                }
                neuronCoord.Add(temp);
            }
        }

        private void DrawingController()
        {
            DrawNetworkCallback d = new DrawNetworkCallback(DrawNetwork);
            while(!IsExitting)
            {
                // Continuously checks the drawing queue and draws anything it finds.
                if (DrawingQueue.Count > 0)
                {
                    Invoke(d, new object[] { DrawingQueue.Dequeue()});
                }
            }
        }

        delegate void DrawNetworkCallback(List<List<Neuron>> layers);

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
            

            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush, 1);

            try
            {
                using (Graphics g = Graphics.FromImage(layout))
                {
                    g.Clear(Color.White);

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
                    LayoutBox.Invalidate();
                    //g.Dispose();
                }
                //layout.Dispose();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            /*
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
                    foreach (double weight in neuron.PrevWeights)
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
                    foreach (double weight in neuron.PrevWeights)
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
                    foreach (double weight in neuron.PrevWeights)
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
            */

            // Saves the current weights for comparison later
            for (int i = 0; i < networkTest.Layers.Count; i++)
            {
                for (int j = 0; j < networkTest.Layers[i].Count; j++)
                {
                    prevWeights[i][j] = networkTest.Layers[i][j].Weights;
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
                List<double> Result = networkTest.Calc(new List<double> { a, b });
                Console.WriteLine("The result of inserting ({0},{1}) is: {2}, {3}", a, b, Result[0], Result[1]);
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