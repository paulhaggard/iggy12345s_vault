using Neuron_Simulation.Activation_Functions;
using Neuron_Simulation.Activation_Functions.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Troschuetz.Random;

namespace Neuron_Simulation
{
    public struct trainingResult
    {
        int iteration;
        int sampleNum;
        List<List<Neuron>> layers;
        double error;

        public int Iteration { get => iteration; set => iteration = value; }
        public int SampleNum { get => sampleNum; set => sampleNum = value; }
        public double Error { get => error; set => error = value; }
        public List<List<Neuron>> Layers { get => layers; set => layers = value; }

        public trainingResult(int iteration, int sampleNum, List<List<Neuron>> layers, double error)
        {
            this.iteration = iteration;
            this.sampleNum = sampleNum;
            this.layers = layers;
            this.error = error;
        }
    }

    public class NeuralNetwork
    {
        // Event Information
        public delegate void TrainingUpdateEventHandler(object sender, TrainingUpdateEventArgs e);

        public event TrainingUpdateEventHandler TrainingUpdateEvent; // Triggered every time this network finishes a sample during training.

        public void OnTrainingUpdateEvent(TrainingUpdateEventArgs e)
        {
            TrainingUpdateEvent?.Invoke(this, e);
        }

        public class TrainingUpdateEventArgs : EventArgs
        {
            int iteration;
            int sampleNum;
            List<List<Neuron>> layers;
            double error;

            public int Iteration { get => iteration; set => iteration = value; }
            public int SampleNum { get => sampleNum; set => sampleNum = value; }
            public double Error { get => error; set => error = value; }
            public List<List<Neuron>> Layers { get => layers; set => layers = value; }

            public TrainingUpdateEventArgs(int iteration, int sampleNum, List<List<Neuron>> layers, double error)
            {
                this.iteration = iteration;
                this.sampleNum = sampleNum;
                this.layers = layers;
                this.error = error;
            }
        }

        // Properties
        private List<List<Neuron>> layers;      // The collection of physical layers of the neural network
        private int neuronCount;
        private int activationCount;
        private bool hasSubscribed = false; // state of whether the network has subscribed to the neurons' activation events or not.
        private double learningRate;
        private Thread trainingThread;
        
        // Constructor
        public NeuralNetwork(List<int> LayerInfo, List<ActivationFunction> defaultActivationFunction = null, List<ActivationParameters> Params = null,
            double learningRate = 0.5)
        {
            // Creates a neural network with LayerInfo.Count layers and each Layer with int neurons.

            this.learningRate = learningRate;

            neuronCount = LayerInfo.Sum();

            layers = new List<List<Neuron>>(LayerInfo.Count);

            if(defaultActivationFunction == null)
            {
                ////Console.WriteLine("Created the default activation functions");
                defaultActivationFunction = new List<ActivationFunction>(LayerInfo.Count);
                for (int i = 0; i < LayerInfo.Count; i++)
                    defaultActivationFunction.Add(new Sigmoid());
            }

            if(Params == null)
            {
                Params = new List<ActivationParameters>(LayerInfo.Count);
                for (int i = 0; i < LayerInfo.Count; i++)
                    Params.Add(new SigmoidParams());
            }

            // Generates the layers of Neurons
            for(int i = 0; i < LayerInfo.Count; i++)
            {
                List<Neuron> temp = new List<Neuron>(LayerInfo[i]);
                if (i == 0)
                    for (int j = 0; j < LayerInfo[i]; j++)
                        temp.Add(new Neuron(defaultActivation: defaultActivationFunction[i], defaultParameters: Params[i]));
                else
                {
                    List<Neuron> prev = layers[i - 1];
                    for (int j = 0; j < LayerInfo[i]; j++)
                        temp.Add(new Neuron(ref prev, defaultActivation: defaultActivationFunction[i], defaultParameters: Params[i]));
                }
                layers.Add(temp);
            }
        }

        // Accessor Methods
        public List<List<Neuron>> Layers { get => layers; set => layers = value; }
        public double LearningRate { get => learningRate; set => learningRate = value; }

        public List<double> Calc(List<double> inputs)
        {
            LoadSample(inputs);
            ForwardPropagate();
            List<double> temp = new List<double>(layers.Last().Count);
            foreach(Neuron neuron in layers.Last())
            {
                temp.Add(neuron.Activation);
            }

            return temp;
        }

        // Training and propagation methods
        public void Train(int iterations, List<List<double>> sample_in, List<List<double>> sample_out, double errorThreshold = 0.01,  bool Reset = false)
        {
            // Trains the neural network

            // Sets up the Normal Distribution random number generator
            NormalDistribution rndNorm = new NormalDistribution();
            rndNorm.Sigma = 0.1;
            rndNorm.Mu = 0;

            // Sets up the binomial distribution random number generator
            BinomialDistribution rndBin = new BinomialDistribution();

            trainingThread = new Thread(new ThreadStart(subTrain));
            trainingThread.Start();

            void subTrain()
            {
                double Error = 0;
                for (int iter = 0; iter < iterations; iter++)
                {
                    // Generates the inital weight and bias tables
                    ////Console.WriteLine("Iteration: {0}", iter);

                    if (Reset)
                    {
                        GenWeightsAndBiases();
                    }

                    if (!hasSubscribed)
                    {
                        // Subscribes to each Activation event of the Neurons
                        Subscribe();
                        ////Console.WriteLine("Subscribed to the neurons!");
                    }

                    // Begins iterations
                    for (int i = 0; i < sample_in.Count; i++)
                    {

                        ////Console.WriteLine("- Sample: {0}", i);

                        LoadSample(sample_in[i]);   // Assigns the inputs

                        ForwardPropagate(); // propagates the network forward

                        Error = BackPropagate(sample_out[i]);    // Backpropagates the network

                        // Sends all of this iteration's data back to the observers
                        TrainingUpdateEventArgs temp = new TrainingUpdateEventArgs(iter, i, layers, Error);

                        OnTrainingUpdateEvent(temp);
                        //if (Error <= errorThreshold)
                            //break;
                    }
                    //if (Error <= errorThreshold)
                        //break;
                }
            }
        }

        public void ForwardPropagate()
        {
            // TODO: figure out how to implement biases into this, since there's one per layer, and is only added once

            // Propagates the network forward, computes an answer

            activationCount = 0;    // Resets the activation count

            // Causes all of the Neurons to fire.
            foreach (Neuron item in layers[0])
            {
                // Creates a personalized thread for each neuron and then activates it.
                Thread ActivationThread = new Thread(new ThreadStart(NeuronActivate));
                ActivationThread.Start();

                void NeuronActivate()
                {
                    item.Activate();
                }
            }

            while (activationCount < neuronCount) ; // Waits until all ActivationFunction are complete
        }

        public double BackPropagate(List<double> Sample)
        {
            // Follows the tutorial found here:
            // https://mattmazur.com/2015/03/17/a-step-by-step-backpropagation-example/
            // For help with understanding the partial derivatives look here:
            // https://sites.math.washington.edu/~aloveles/Math126Spring2014/PartialDerivativesPractice.pdf

            // ^ Is out of date, use this instead now vvv
            // http://pandamatak.com/people/anand/771/html/node37.html
            // And this one for bias back propagation
            // https://theclevermachine.wordpress.com/2014/09/06/derivation-error-backpropagation-gradient-descent-for-neural-networks/

            // Propagates the network backward, uses computed answers, compared to real answers, to update the weights and biases
            // Returns the %error the this training sample

            // Computes the cost of the last layer's results (%error) --> Cost = ((out - expected)^2)/2
            List<double> Costs = new List<double>(layers.Last().Count);
            double CostTotal = 0;
            for (int i = 0; i < layers.Last().Count; i++)
                Costs.Add(Math.Pow(layers.Last()[i].Activation - Sample[i], 2)/2);
            foreach (double item in Costs)
                CostTotal += item;

            List<double> DeltaK = new List<double>(layers.Last().Count);  // Creates a list of Deltailons used for the output layers.
            for (int i = 0; i < layers.Last().Count; i++)
                DeltaK.Add(layers.Last()[i].DefaultActivation.Derivate(layers.Last()[i].Net, layers.Last()[i].DefaultParameters) * (Sample[i] - layers.Last()[i].Activation));

            List<List<double>> DeltaH = new List<List<double>>(layers.Count);   // Creates a 2-dimensional map of every weight in the matrix.
            List<double> DeltaB = new List<double>(layers.Count);               // Creates a map for the biases in the network.

            for (int i = layers.Count - 1; i > 0; i--)
            {
                // Does the physical backpropagation
                DeltaH.Add(new List<double>(layers[i].Count));
                for(int j = 0; j < layers[i].Count; j++)
                {
                    for(int k = 0; k < layers[i][j].Weight_in.Count; k++)
                    {
                        /* Variable meanings:
                         * i = current layer
                         * j = current neuron of current layer
                         * k = current input weight of current neuron from current layer
                         * l = current neuron from next layer
                         */

                        double sum = 0;
                        if (i == layers.Count - 1)
                        {
                            // Back propagates the output layer
                            DeltaH[(layers.Count - 1) - i].Add(DeltaK[j]);
                            sum += layers[i-1][k].Activation * DeltaK[j];
                            layers[i][j].Bias -= DeltaK[j];
                            layers[i][j].Weight_in[k] -= learningRate * DeltaH[(layers.Count - 1) - i][j]; //* layers[i - 1][k].Activation;
                        }
                        else
                        {
                            for (int l = 0; l < layers[i + 1].Count; l++)
                            {
                                // Sums up all of the weights downstream from layer i, neuron j, weight k
                                sum += layers[i + 1][l].Weight_in[j] * DeltaH[((layers.Count - 1) - i) - 1][l];
                            }
                            // Calculates the delta for this weight on this neuron
                            DeltaH[(layers.Count - 1) - i].Add(sum * layers[i][j].DefaultActivation.Derivate(layers[i][j].Net, layers[i][j].DefaultParameters));
                            // assigns said delta to the weight if the current layer isn't the input layer
                            layers[i][j].Weight_in[k] -= learningRate * DeltaH[(layers.Count - 1) - i][j] * layers[i - 1][k].Activation;
                            // Adjusts the bias
                            layers[i][j].Bias -= DeltaH[(layers.Count - 1) - i][j];
                        }
                    }
                }
            }

            for(int i = 0; i < layers[0].Count; i++)
            {
                // Performs back propagation on the input layer biases

                double sum = 0;
                for (int l = 0; l < layers[1].Count; l++)
                {
                    // Sums up all of the weights downstream from layer i, neuron j, weight k
                    sum += layers[1][l].Weight_in[i] * DeltaH[layers.Count - 2][l];
                }

                // Adjusts the bias
                layers[0][i].Bias -= sum * layers[0][i].DefaultActivation.Derivate(layers[0][i].Net, layers[0][i].DefaultParameters);
            }

            
            return CostTotal;
        }

        public void GenWeightsAndBiases()
        {
            // Can allow the controller to generate the biases and weights prior to training.

            // Sets up the Normal Distribution random number generator
            NormalDistribution rndNorm = new NormalDistribution();
            rndNorm.Sigma = 0.05;
            rndNorm.Mu = 0;

            // Sets up the binomial distribution random number generator
            BinomialDistribution rndBin = new BinomialDistribution();

            // Assigns the biases, and weights
            for (int j = 1; j < layers.Count; j++)
            {
                for (int k = 0; k < layers[j].Count; k++)
                {
                    // Initializes the network's biases and weights
                    
                    List<double> temp = new List<double>(layers[j][k].Weight_in.Capacity);
                    for (int l = 0; l < layers[j][k].Weight_in.Capacity; l++)
                        temp.Add(rndNorm.NextDouble());
                    layers[j][k].Weight_in = temp;
                    layers[j][k].Bias = rndBin.NextDouble();
                }
            }
        }

        private void OnActiveEvent(object sender, EventArgs e)
        {
            activationCount++; // symbolizes that a neuron has fired
        }

        public void LoadSample(List<double> Sample)
        {
            for (int i = 0; i < layers[0].Count; i++)
            {
                layers[0][i].Inputs[0] = Sample[i];
            }
        }

        public void Subscribe()
        {
            // Causes the neural network to subscribe to all of it's neuron's activation events
            // Subscribes to each Activation event of the Neurons
            for (int i = 0; i < layers.Count; i++)
                for (int j = 0; j < layers[i].Count; j++)
                    layers[i][j].ActiveEvent += OnActiveEvent;
        }
    }
}
