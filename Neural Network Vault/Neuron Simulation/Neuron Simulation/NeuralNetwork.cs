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
        private double learningRate;
        
        // Constructor
        public NeuralNetwork(List<int> LayerInfo, List<ActivationFunction> defaultActivationFunction = null, List<ActivationParameters> Params = null,
            double learningRate = 0.01)
        {
            // Creates a neural network with LayerInfo.Count layers and each Layer with int neurons.

            this.learningRate = learningRate;

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

        // Accessor Methods
        public List<List<Neuron>> Layers { get => layers; set => layers = value; }
        public double LearningRate { get => learningRate; set => learningRate = value; }

        public void Train(int iterations, List<List<double>> sample_in, List<List<double>> sample_out, List<List<List<double>>> weight_init = null, List<double> bias_init = null)
        {
            // Trains the neural network

            // Sets up the Normal Distribution random number generator
            NormalDistribution rndNorm = new NormalDistribution();
            rndNorm.Sigma = 0.1;
            rndNorm.Mu = 0;

            // Sets up the binomial distribution random number generator
            BinomialDistribution rndBin = new BinomialDistribution();

            for (int iter = 0; iter < iterations; iter++)
            {
                // Generates the inital weight and bias tables

                // Generates a random weight table if one wasn't supplied
                if (weight_init == null)
                {
                    weight_init = new List<List<List<double>>>(Layers.Count);
                    for (int i = 0; i < Layers.Count; i++)
                    {
                        List<List<double>> temp = new List<List<double>>(Layers[i].Count);
                        for (int j = 0; j < Layers[i].Count; j++)
                        {
                            int currentIndex;
                            if (i == 0)
                                currentIndex = 1;
                            else
                                currentIndex = Layers[i - 1].Count;
                            List<double> temp2 = new List<double>(currentIndex);
                            for (int k = 0; k < currentIndex; k++)
                                temp2.Add(rndNorm.NextDouble());
                            temp.Add(temp2);
                        }
                        weight_init.Add(temp);
                    }
                }

                // Generates a random bias table if one wasn't supplied
                if (bias_init == null)
                {
                    bias_init = new List<double>(sample_in.Count);
                    for (int i = 0; i < sample_in.Count; i++)
                    {
                        bias_init.Add(rndBin.NextDouble());
                    }
                }

                // Subscribes to each Activation event of the Neurons
                for (int i = 0; i < layers.Count; i++)
                    for (int j = 0; j < layers[i].Count; j++)
                        layers[i][j].ActiveEvent += OnActiveEvent;

                // Begins iterations
                for (int i = 0; i < sample_in.Count; i++)
                {
                    activationCount = 0; // Resets the activationCount
                                         //dCost = 0;

                    // Assigns the biases, and weights, and inputs
                    for (int j = 0; j < layers.Count; j++)
                    {
                        for (int k = 0; k < layers[i].Count; k++)
                        {
                            layers[i][k].Bias_in = bias_init[j];
                            layers[i][k].Weight_in = weight_init[j][k];

                            if (j == 0)
                            {
                                layers[i][k].Inputs[0] = sample_in[i][k];
                            }
                        }
                    }

                    ForwardPropagate(); // propagates the network forward

                    
                }
            }
        }

        public void ForwardPropagate()
        {
            // Propagates the network forward, computes an answer

            // Causes all of the Neurons to fire.
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

            // Propagates the network backward, uses computed answers, compared to real answers, to update the weights and biases
            // Returns the %error the this training sample

            // Computes the cost of the last layer's results (%error) --> Cost = ((out - expected)^2)/2
            List<double> Costs = new List<double>(layers.Last().Count);
            double CostTotal = 0;
            for (int i = 0; i < layers.Last().Count; i++)
                Costs.Add(Math.Pow(layers.Last()[i].Activation - Sample[i], 2)/2);
            foreach (double item in Costs)
                CostTotal += item;

            // Backpropagates the output layer
            for(int i = 0; i < layers.Last().Count; i++)
            {
                double dEdO = -(Sample[i] - layers.Last()[i].Activation);                               // Calculates the partial derivative of the total error with respect to the output
                double dOdN = layers.Last()[i].DefaultActivation.Derivate(layers.Last()[i].Activation,  // Calculates the partial derivative of the output with respect to the net activation
                    layers.Last()[i].DefaultParameters);
                for (int j = 0; j < layers[layers.Count - 2].Count; j++)
                {
                    double dNdW = layers[layers.Count - 2][j].Activation;                                // Calculates the partial derivative of the net activation with respect to the weight
                    double dEdW = dEdO * dOdN * dNdW;                                                    // Chain rules it all together
                    // ^ This is the amount we would need to subtract from the current rate to make the output be correct for this sample if it came across again
                    // But, then it wouldn't work for any other sample, so we need to multiply it by some learning rate to decrease it's impact a bit.

                    layers.Last()[i].Weight_in[j] -= dEdW * learningRate;
                }
            }



            /*
            // Computes the slope of all of the layers (How much they affect the output layer's neurons)
            List<List<double>> slope_layer = new List<List<double>>(layers.Count);

            for (int i = 0; i < layers.Last().Count; i++)
            {
                slope_layer.Add(new List<double>(layers[i].Count));
                for (int j = 0; j < layers[i].Count; j++)
                {
                    slope_layer[i].Add(layers.Last()[i].DefaultActivation.Derivate(
                        layers.Last()[i].Activation, layers.Last()[i].DefaultParameters));
                }
            }

            // Computes the change factor of the weights at each layer
            List<List<double>> change_factors = new List<List<double>>(layers.Count);
            List<double> E = Costs;

            // START HERE: https://www.analyticsvidhya.com/blog/2017/05/neural-network-from-scratch-in-python-and-r/
            // AND HERE: https://www.analyticsvidhya.com/blog/2017/03/introduction-to-gradient-descent-algorithm-along-its-variants/

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                double temp = 0;
                change_factors.Add(new List<double>(layers[i].Count));
                for (int j = 0; j < layers[i].Count; j++)
                {
                    change_factors[layers.Count - i].Add(E[j]*slope_layer[i][j]);
                    temp += change_factors[layers.Count - i][j] * layers[i][j].Weight_in;
                }
            }



            double Z = 0;
            double dActFnc = 0;
            double dCost = 0;
            double dCdW = 0;
            double dCdb = 0;
            for(int i = 0; i < layers.Count; i++)
            {
                for(int j = 0; j < layers[i].Count; j++)
                {
                    // Computes the Z of the current layer's neuron --> Z = Sum(Weight * Z(L-1)) + Bias
                    Z = layers[i][j].Bias_in;
                    for (int k = 0; k < layers[i - 1].Count; k++)
                        Z += layers[i][j].Weight_in * layers[i - 1][k].Activation;

                    // Computes the derivative of the activation function of this layer
                    dActFnc = layers[i][j].DefaultActivation.Derivate(Z, layers[i][j].DefaultParameters);

                    // Computes the derivative of the cost function with respect to a(L) --> = 2(out - expected)
                    dCost = 2 * (layers[i][j].Activation - Sample[j]);

                    // Compute the ratios of the corresponding weights and biases
                    dCdW = layers[i][j].InputNeurons.Sum()

                }
            }
            */

            return Costs.Sum() / Costs.Count;
        }

        private void OnActiveEvent(object sender, EventArgs e)
        {
            activationCount++; // symbolizes that a neuron has fired
        }
    }
}
