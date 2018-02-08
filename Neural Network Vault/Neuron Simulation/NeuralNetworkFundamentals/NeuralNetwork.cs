using NeuralNetworkFundamentals.Activation_Functions;
using NeuralNetworkFundamentals.Activation_Functions.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Troschuetz.Random;

namespace NeuralNetworkFundamentals
{
    public class TrainingUpdateEventArgs : EventArgs
    {
        int iteration;
        int sampleNum;
        List<List<Neuron>> layers;
        double error;
        bool finished;

        public int Iteration { get => iteration; set => iteration = value; }
        public int SampleNum { get => sampleNum; set => sampleNum = value; }
        public double Error { get => error; set => error = value; }
        public List<List<Neuron>> Layers { get => layers; set => layers = value; }
        public bool Finished { get => finished; set => finished = value; }

        public TrainingUpdateEventArgs(int iteration, int sampleNum, List<List<Neuron>> layers, double error, bool finished)
        {
            this.iteration = iteration;
            this.sampleNum = sampleNum;
            this.layers = layers;
            this.error = error;
            this.finished = finished;
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

        public delegate void TrainingFinishEventHandler(object sender);

        public event TrainingFinishEventHandler TrainingFinishEvent; // Triggered every time this network finishes training.

        public void OnTrainingFinishEvent()
        {
            TrainingFinishEvent?.Invoke(this);
        }

        // Properties
        private List<List<Neuron>> layers;      // The collection of physical layers of the neural network
        private int neuronCount;
        private int activationCount;
        private bool hasSubscribed = false; // state of whether the network has subscribed to the neurons' activation events or not.
        private double learningRate;
        private double momentum;
        private Thread trainingThread;

        // Constructor
        public NeuralNetwork(List<int> LayerInfo, List<ActivationFunction> defaultActivationFunction = null, List<ActivationParameters> Params = null,
            double learningRate = 0.5, double momentum = 0)
        {
            // Creates a neural network with LayerInfo.Count layers and each Layer with int neurons.

            this.learningRate = learningRate;
            this.momentum = momentum;

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
                        temp.Add(new Neuron(defaultActivation: defaultActivationFunction[i], defaultParameters: Params[i]));        // Creates the input layer
                else
                {
                    List<Neuron> prev = layers[i - 1];
                    for (int j = 0; j < LayerInfo[i]; j++)
                        temp.Add(new Neuron(prev,
                            defaultActivation: defaultActivationFunction[i],
                            defaultParameters: Params[i],
                            outputLayer: (i == LayerInfo.Count - 1)));  // Generates the rest of the layers
                }
                layers.Add(temp);
            }
        }

        // Accessor Methods
        public List<List<Neuron>> Layers { get => layers; set => layers = value; }
        public double LearningRate { get => learningRate; set => learningRate = value; }

        public List<double> Calc(List<double> inputs)
        {
            // Runs the network through its forward cycle and returns the outputs
            LoadSample(inputs);
            ForwardPropagate();
            List<double> temp = new List<double>(layers.Last().Count);
            foreach(Neuron neuron in layers.Last())
            {
                temp.Add(neuron.Activation);
            }

            return temp;
        }

        public void GenWeightsAndBiases(List<List<List<double>>> weights = null, List<List<double>> biases = null)
        {
            // Can allow the controller to generate the biases and weights prior to training.

            // Sets up the Normal Distribution random number generator
            NormalDistribution rndNorm = new NormalDistribution();
            rndNorm.Sigma = 0.5;
            rndNorm.Mu = 0;

            // Sets up the binomial distribution random number generator
            BinomialDistribution rndBin = new BinomialDistribution();

            // Assigns the biases, and weights
            for (int i = 0; i < layers.Count; i++)
            {
                for (int j = 0; j < layers[i].Count; j++)
                {
                    // Initializes the network's biases and weights
                    if (weights == null)
                        layers[i][j].RandomizeWeights(rndNorm);
                    else
                        layers[i][j].Weights = weights[i][j];

                    if (biases == null)
                        layers[i][j].RandomizeBias(rndBin);
                    else
                        layers[i][j].Bias = biases[i][j];
                }
            }
        }

        public List<List<List<double>>> Weights { get => GetWeights(); set => GenWeights(value); }
        public List<List<double>> Biases { get => GetBiases(); set => GenBiases(value); }

        private void GenWeights(List<List<List<double>>> weights = null)
        {
            // Can allow the controller to generate the biases and weights prior to training.

            // Sets up the Normal Distribution random number generator
            NormalDistribution rndNorm = new NormalDistribution();
            rndNorm.Sigma = 0.05;
            rndNorm.Mu = 0;

            // Assigns the biases, and weights
            for (int i = 1; i < layers.Count - 1; i++)
            {
                for (int j = 0; j < layers[i].Count; j++)
                {
                    // Initializes the network's biases and weights

                    if (weights == null)
                        layers[i][j].RandomizeWeights(rndNorm);
                    else
                        layers[i][j].Weights = weights[i][j];
                }
            }
        }

        private void GenBiases(List<List<double>> biases = null)
        {
            // Can allow the controller to generate the biases and weights prior to training.

            // Sets up the binomial distribution random number generator
            BinomialDistribution rndBin = new BinomialDistribution();

            // Assigns the biases, and weights
            for (int i = 0; i < layers.Count; i++)
            {
                for (int j = 0; j < layers[i].Count; j++)
                {
                    // Initializes the network's biases and weights
                    if (biases == null)
                        layers[i][j].RandomizeBias(rndBin);
                    else
                        layers[i][j].Bias = biases[i][j];
                }
            }
        }

        private List<List<List<double>>> GetWeights()
        {
            List<List<List<double>>> temp = new List<List<List<double>>>(layers.Count);
            for (int i = 1; i < layers.Count; i++)
            {
                temp.Add(new List<List<double>>(layers[i].Count));
                for (int j = 0; j < layers[i].Count; j++)
                {
                    temp[i].Add(layers[i][j].Weights);
                }
            }
            return temp;
        }

        private List<List<double>> GetBiases()
        {
            List<List<double>> temp = new List<List<double>>(layers.Count);
            for (int i = 0; i < layers.Count; i++)
            {
                temp.Add(new List<double>(layers[i].Count));
                for (int j = 0; j < layers[i].Count; j++)
                {
                    temp[i].Add(layers[i][j].Bias);
                }
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
                TrainingUpdateEventArgs temp;
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

                        //Console.WriteLine("- Sample: {0}", i);

                        LoadSample(sample_in[i]);   // Assigns the inputs

                        ForwardPropagate(); // propagates the network forward

                        Error = BackPropagate(sample_out[i]);    // Backpropagates the network

                        // Sends all of this iteration's data back to the observers
                        temp = new TrainingUpdateEventArgs(iter, i, layers, Error, false);

                        OnTrainingUpdateEvent(temp);
                        //if (Error <= errorThreshold)
                            //break;
                    }
                    //if (Error <= errorThreshold)
                        //break;
                }
                OnTrainingFinishEvent();    // Sends out an event notifying that training has completed.
            }
        }

        public void ForwardPropagate()
        {
            // Propagates the network forward, computes an answer

            activationCount = 0;    // Resets the activation count

            foreach (Neuron item in layers[0])
            {
                item.Activate();
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

            // ^ Is out of date, use this instead now vvv
            // XOR Example project

            // Propagates the network backward, uses computed answers, compared to real answers, to update the weights and biases
            // Returns the %error the this training sample

            // START HERE: https://youtu.be/An5z8lR8asY

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                for(int j = 0; j < layers[i].Count; j++)
                {
                    if(i == layers.Count - 1)
                    {

                    }
                }
            }

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                // Does the physical backpropagation
                for(int j = 0; j < layers[i].Count; j++)
                {
                    /* Variable meanings:
                         * i = current layer
                         * j = current neuron of current layer
                         */

                    if (i == layers.Count - 1)
                        layers[i][j].AssignError(momentum, learningRate, Sample[j]);
                    else
                        layers[i][j].AssignError(momentum, learningRate, nextLayerNeurons: layers[i + 1]);
                }
            }

            // Calculates the total error that the networkw as off by
            double ErrorTotal = 0;

            for (int i = 0; i < layers.Last().Count; i++)
                ErrorTotal += Math.Pow(Sample[i] - layers.Last()[i].Activation, 2) / 2;

            
            return ErrorTotal;
        }

        private void OnActiveEvent(object sender, EventArgs e)
        {
            activationCount++; // symbolizes that a neuron has fired
        }

        public void LoadSample(List<double> Sample)
        {
            for (int i = 0; i < layers[0].Count; i++)
            {
                layers[0][i].RawInput = Sample[i];
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
