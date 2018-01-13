using Neuron_Simulation.Activation_Functions;
using Neuron_Simulation.Activation_Functions.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;

namespace Neuron_Simulation
{
    class NeuralNetwork
    {
        // Properties
        private List<List<Neuron>> layers;      // The collection of physical layers of the neural network
        private int neuronCount;
        private int activationCount;
        
        // Constructor
        public NeuralNetwork(List<int> LayerInfo, List<ActivationFunction> defaultActivationFunction = null, List<ActivationParameters> Params = null)
        {
            // Creates a neural network with LayerInfo.Count layers and each Layer with int neurons.

            neuronCount = LayerInfo.Sum();

            Layers = new List<List<Neuron>>(LayerInfo.Count);

            if(defaultActivationFunction == null)
            {
                defaultActivationFunction = new List<ActivationFunction>(LayerInfo.Count);
                for (int i = 0; i < defaultActivationFunction.Count; i++)
                    defaultActivationFunction[i] = new Sigmoid();
            }

            if(Params == null)
            {
                Params = new List<ActivationParameters>(LayerInfo.Count);
                for (int i = 0; i < Params.Count; i++)
                    Params[i] = new SigmoidParams();
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
                    List<Neuron> prev = Layers[i - 1];
                    for (int j = 0; j < LayerInfo[i]; j++)
                        temp.Add(new Neuron(ref prev, defaultActivation: defaultActivationFunction[i], defaultParameters: Params[i]));
                }
            }
        }

        public List<List<Neuron>> Layers { get => layers; set => layers = value; }

        public void Train(List<List<double>> sample_in, List<List<double>> sample_out, List<List<double>> weight_init = null, List<List<double>> bias_init = null)
        {
            // Trains the neural network

            // Setup training variables

            double cost = 0;    // This is the error function for a specific training set
            double dCost = 0;   // This is the error of the network for an entire iteration

            // Sets up the Normal Distribution random number generator
            NormalDistribution rndNorm = new NormalDistribution();
            rndNorm.Sigma = 0.1;
            rndNorm.Mu = 0;

            // Sets up the binomial distribution random number generator
            BinomialDistribution rndBin = new BinomialDistribution();

            // Generates the inital weight and bias tables

            // Generates a random weight table if one wasn't supplied
            if(weight_init == null)
            {
                weight_init = new List<List<double>>(sample_in.Count);
                for(int i = 0; i < sample_in.Count; i++)
                {
                    List<double> temp = new List<double>(layers[i].Count);
                    for (int j = 0; j < layers[i].Count; j++)
                        temp.Add(rndNorm.NextDouble());
                    weight_init.Add(temp);
                }
            }

            // Generates a random bias table if one wasn't supplied
            if(bias_init == null)
            {
                bias_init = new List<List<double>>(sample_in.Count);
                for (int i = 0; i < sample_in.Count; i++)
                {
                    List<double> temp = new List<double>(layers[i].Count);
                    for (int j = 0; j < layers[i].Count; j++)
                        temp.Add(rndBin.NextDouble());
                    weight_init.Add(temp);
                }
            }

            // Subscribes to each Activation event of the Neurons
            for (int i = 0; i < layers.Count; i++)
                for (int j = 0; j < layers[i].Count; j++)
                    layers[i][j].ActiveEvent += OnActiveEvent;

            // Begins iterations
            for(int i = 0; i < sample_in.Count; i++)
            {
                activationCount = 0; // Resets the activationCount
                //dCost = 0;

                // Assigns the biases, and weights, and inputs
                for (int j = 0; j < layers.Count; j++)
                {
                    for (int k = 0; k < layers[i].Count; k++)
                    {
                        layers[i][k].Bias_in = bias_init[j][k];
                        layers[i][k].Weight_in = weight_init[j][k];

                        if(j == 0)
                        {
                            layers[i][k].Inputs[0] = sample_in[i][k];
                        }
                    }
                }

                // Causes all of the Neurons to fire.
                foreach(Neuron item in layers[0])
                {
                    item.Activate();
                }

                while (activationCount < neuronCount) ; // Waits until all ActivationFunction are complete

                // Computes the result of the Neural network
                double temp = 0;
                for(int j = 0; j < layers.Last().Count; j++)
                {
                    // Compare the outputs of the network versus the expected output of the network and square them
                    temp += Math.Pow(layers.Last()[j].Activation - sample_out[i][j], 2);
                }

                // Compute the cost of training
                cost = temp / Layers.Last().Count;

                // Back propagation time!
                // https://youtu.be/tIeHLnjs5U8

                dCost += 
            }
        }

        private void OnActiveEvent(object sender, EventArgs e)
        {
            activationCount++; // symbolizes that a neuron has fired
        }
    }
}
