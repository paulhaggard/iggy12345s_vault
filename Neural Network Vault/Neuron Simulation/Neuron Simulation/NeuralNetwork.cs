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
        private bool hasSubscribed = false; // state of whether the network has subscribed to the neurons' activation events or not.
        private double learningRate;
        
        // Constructor
        public NeuralNetwork(List<int> LayerInfo, List<ActivationFunction> defaultActivationFunction = null, List<ActivationParameters> Params = null,
            double learningRate = 0.01)
        {
            // Creates a neural network with LayerInfo.Count layers and each Layer with int neurons.

            this.learningRate = learningRate;

            neuronCount = LayerInfo.Sum();

            layers = new List<List<Neuron>>(LayerInfo.Count);

            if(defaultActivationFunction == null)
            {
                Console.WriteLine("Created the default activation functions");
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

        public int Calc(List<double> inputs)
        {
            LoadSample(inputs);
            ForwardPropagate();
            int temp = 0;
            int iter = 0;
            foreach (Neuron item in layers.Last())
            {
                temp = (item.Activation > temp) ? iter : temp;
                iter++;
            }

            return temp;
        }

        // Training and propagation methods
        public void Train(int iterations, List<List<double>> sample_in, List<List<double>> sample_out, bool Reset = false, List<List<List<double>>> weight_init = null, List<double> bias_init = null)
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

                if (Reset)
                {
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
                }

                if (!hasSubscribed)
                {
                    // Subscribes to each Activation event of the Neurons
                    for (int i = 0; i < layers.Count; i++)
                        for (int j = 0; j < layers[i].Count; j++)
                            layers[i][j].ActiveEvent += OnActiveEvent;
                }

                // Begins iterations
                for (int i = 0; i < sample_in.Count; i++)
                {
                    activationCount = 0; // Resets the activationCount
                                         //dCost = 0;

                    // Assigns the biases, and weights
                    for (int j = 0; j < layers.Count; j++)
                    {
                        for (int k = 0; k < layers[i].Count; k++)
                        {
                            // Re-initializes the network's biases and weights if the reset boolean is true and this is the first iteration
                            if ((iter == 0)&&Reset)
                            {
                                layers[i][k].Bias_in = bias_init[j];
                                layers[i][k].Weight_in = weight_init[j][k];
                            }
                        }
                    }

                    LoadSample(sample_in[i]);   // Assigns the inputs

                    ForwardPropagate(); // propagates the network forward

                    BackPropagate(sample_in[i]);    // Backpropagates the network
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
            // also generates lists of derivative solutions that are used in later calculations
            List<double> dEdO = new List<double>(layers.Last().Count);
            List<double> dOdN = new List<double>(layers.Last().Count);
            for (int i = 0; i < layers.Last().Count; i++)
            {
                dEdO.Add(-(Sample[i] - layers.Last()[i].Activation));                               // Calculates the partial derivative of the total error with respect to the output
                dOdN.Add(layers.Last()[i].DefaultActivation.Derivate(layers.Last()[i].Activation,  // Calculates the partial derivative of the output with respect to the net activation
                    layers.Last()[i].DefaultParameters));
                for (int j = 0; j < layers[layers.Count - 2].Count; j++)
                {
                    double dNdW = layers[layers.Count - 2][j].Activation;                                // Calculates the partial derivative of the net activation with respect to the weight
                    double dEdW = dEdO[i] * dOdN[i] * dNdW;                                                    // Chain rules it all together
                    // ^ This is the amount we would need to subtract from the current rate to make the output be correct for this sample if it came across again
                    // But, then it wouldn't work for any other sample, so we need to multiply it by some learning rate to decrease it's impact a bit.

                    layers.Last()[i].Weight_in[j] -= dEdW * learningRate;
                }
            }

            // Back propagates the rest of the layers for their weight and bias values
            for(int i = layers.Count - 2; i > 0; i--)
            {
                // Run backwards through every layer and propagate them using the math example from the 'Back Propagation Math' folder's pictures
                // Must iterate for every layer starting from the second to last one, and stopping at the layer before the input layer

                // Back propagates the weight and bias values
                for(int j = 0; j < layers[i].Count; j++)
                {
                    // calculate the partial derivatives for each of the weights attached to each neuron in the layer
                    for(int k = 0; k < layers[i][j].Weight_in.Count; k++)
                    {
                        double dEdOl = 0;   // Derivative of the total error with respect to the output of the current layer
                        double dOdNl = 0;   // Derivative of the ouptut of the current layer with respect to the net output of the current layer
                        double dNdWl = 0;   // Derivative of the net ouput of the current layer with respect to the weight in question

                        // Calculates dEdOl
                        for(int l = 0; l < layers.Last().Count; l++)
                        {
                            // calculates the partial derivative of the total error with respect to the output of the neuron attached to the current weight

                            double dEldOl = dEdO[l]*dOdN[l];  // Derivative of the current output neuron's error with respect to the output of the layer in question
                            // ^ must be chained with every neuron and layer that it comes in contact with that uses the weight in question in it's output calculation

                            // We can't iterate on the output layer because it's essential to calculation so if the current layer is equal to the second to last layer
                            // Then do this instead.
                            if(i == layers.Count - 2)
                                dEldOl *= layers.Last()[l].Weight_in[j];    // Derivative of dNet/dOut
                            else
                            {
                                // Otherwise we need to iterate through and chain together every layer betwen the current one and the output
                                for(int m = layers.Count - 2; m > i; m--)
                                {
                                    if (m == layers.Count - 2)
                                        dEldOl *= layers.Last()[l].Weight_in[j];    // Derivative of dNet/dOut
                                    else
                                    {
                                        //
                                        for(int n = 0; n < layers[m].Count; n++)
                                        {
                                            /*  i = the current layer
                                             *  j = the current neuron in the current layer
                                             *  k = the current weight of the current neuron on the current layer
                                             *  l = the current ouput neuron of the ouput layer
                                             *  m = the current layer between the output layer and the current layer (i)
                                             *  n = the current neuron of the current layer between the output layer and the current layer (m)
                                             *  o = the current neuron of the next layer between the output layer and the current layer (m + 1)
                                             */

                                            double dNdOl = 1;
                                            for(int o = 0; o < layers[m+1].Count; o++)
                                            {
                                                // Chains the weights of all of the next layer's neurons that connect with this one (all of them)
                                                dNdOl *= layers[m + 1][o].Weight_in[n];
                                            }

                                            // Chains the derivative of the activation function of each neuron in the current layer m
                                            double dOldNl = layers[i][j].DefaultActivation.Derivate(layers[i][j].Net, layers[i][j].DefaultParameters);

                                            // If the layer isn't the layer right next to the current layer i, then we don't need to chain the weights of layer m and layer i
                                            // Because that'll be covered by the next iteration anyway.
                                            if(m == i + 1)
                                            {
                                                dOldNl *= layers[m][n].Weight_in[j];
                                            }

                                            // Chaining it all together
                                            dEldOl *= dNdOl * dOldNl;
                                        }
                                    }
                                }
                            }

                            dEdOl += dEldOl;    // Sums up the derivatives of all of the output layer's errors with respect to the output of the layer in question
                        }

                        // Calculates dOdNl
                        dOdNl = layers[i][j].DefaultActivation.Derivate(layers[i][j].Net, layers[i][j].DefaultParameters);

                        // Calculates dNdWl
                        dNdWl = layers[i - 1][j].Activation;

                        // Chains it all together
                        double dEdW = dEdOl * dOdNl * dNdWl;

                        // Updates the weight of the current neuron
                        layers[i][j].Weight_in[k] -= dEdW * learningRate;

                        // Back propagates the bias values
                        // This uses all of the values from the previous section, except dNdWl = 1
                        layers[i][j].Bias_in -= dEdOl * dOdNl * learningRate;
                    }
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

            return CostTotal;
        }

        private void OnActiveEvent(object sender, EventArgs e)
        {
            activationCount++; // symbolizes that a neuron has fired
        }

        private void LoadSample(List<double> Sample)
        {
            for (int i = 0; i < layers[0].Count; i++)
            {
                layers[0][i].Inputs[0] = Sample[i];
            }
        }
    }
}
