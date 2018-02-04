using System;
using System.Collections.Generic;
using System.Linq;

namespace Debugging_Console_App
{ 
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Setting up test...");
            List<List<Neuron>> layers = new List<List<Neuron>>(3);  // List of layers containing neurons in the network

            for(int i = 0; i < 3; i++)
            {
                // Populates the network with neurons
                // sets the values according to this website:
                // https://mattmazur.com/2015/03/17/a-step-by-step-backpropagation-example/

                switch (i)
                {
                    case 0:
                        layers.Add(new List<Neuron>(2) { new Neuron(), new Neuron() }); // Creates the input layer with 2 neurons
                        break;

                    case 1:
                        layers.Add(new List<Neuron>(2) { new Neuron(layers[0], new List<double>(2) { 0.15, 0.2 }, 0.35),
                            new Neuron(layers[0], new List<double>(2) { 0.25, 0.3 }, 0.35)});    // Creates the first hidden layer with 2 neurons
                        break;

                    case 2:
                        layers.Add(new List<Neuron>(2) { new Neuron(layers[1], new List<double>(2) { 0.4, 0.45 }, 0.6),
                            new Neuron(layers[0], new List<double>(2) { 0.5, 0.55 }, 0.6)});    // Creates the output layer with 2 neurons
                        break;
                }
            }

            Console.WriteLine("Testing the feed forward network");

            List<double> inputs = new List<double>(2) { 0.05, 0.1 };    // The inputs to the network
            List<double> nextLayerInput = new List<double>(2) { 0, 0 };

            // Feeds the network forward
            for (int iteration = 0; iteration < 10000; iteration++)
            {
                Console.WriteLine("Iteration {0}", iteration);
                                                                            // Sets the inputs of each of the neurons to the corresponding input from the list
                layers[0][0].rawInput = inputs[0];
                layers[0][1].rawInput = inputs[1];

                for (int i = 0; i < 3; i++)
                {
                    // Activates the current layer's neurons
                    nextLayerInput[0] = layers[i][0].Activate();
                    nextLayerInput[1] = layers[i][1].Activate();
                    // Sets the inputs for the next layer, unless the current layer is the output layer
                    if (i == 2)
                        break;
                    // sets the next layer's first neuron's inputs
                    layers[i + 1][0].inputs[0] = layers[i][0];
                    layers[i + 1][0].inputs[1] = layers[i][1];
                    // sets the next layer's second neuron's inputs
                    layers[i + 1][1].inputs[0] = layers[i][0];
                    layers[i + 1][1].inputs[1] = layers[i][1];
                }

                Console.WriteLine("The test output is: [{0}, {1}]", nextLayerInput[0], nextLayerInput[1]);

                // Calculates the total error
                List<double> outputs = new List<double>(2) { 0.01, 0.99 };
                double Error1 = Math.Pow(nextLayerInput[0] - outputs[0], 2) / 2;
                double Error2 = Math.Pow(nextLayerInput[1] - outputs[1], 2) / 2;

                Console.WriteLine("The Errors were: {0} and {1}\n", Error1, Error2);

                // Performs the back propagation

                double learningRate = 0.5;

                List<double> DeltaK = new List<double>(layers.Last().Count);  // Creates a list of DeltaK used for the output layers.
                for (int i = 0; i < layers.Last().Count; i++)
                {
                    DeltaK.Add(layers.Last()[i].Derivate() * (layers.Last()[i].activation - outputs[i]));
                    Console.WriteLine("dE/dOut of neuron {0}: {1}", i, (layers.Last()[i].activation - outputs[i]));
                    Console.WriteLine("dOut/dNet of neuron {0}: {1}", i, layers.Last()[i].Derivate());
                    Console.WriteLine("Delta K of neuron {0}: {1}\n", i, DeltaK[i]);
                }

                List<List<double>> DeltaH = new List<List<double>>(layers.Count);   // Creates a 2-dimensional map of every weight in the matrix.
                List<double> DeltaB = new List<double>(layers.Count);               // Creates a map for the biases in the network.

                for (int i = layers.Count - 1; i > 0; i--)
                {
                    // Does the physical backpropagation
                    Console.WriteLine("Layer {0}", i);
                    DeltaH.Add(new List<double>(layers[i].Count));
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        for (int k = 0; k < layers[i][j].weights.Count; k++)
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
                                sum += layers[i - 1][k].activation * DeltaK[j];
                                layers[i][j].bias -= learningRate * DeltaK[j];
                                layers[i][j].weights[k] -= learningRate * DeltaH[(layers.Count - 1) - i][j] * sum; //* layers[i - 1][k].Activation;
                                Console.WriteLine("dNet/dW of neuron {0} weight {1}: {2}", j, k, layers[i - 1][k].activation);
                                Console.WriteLine("Adjustment to output {0} weight {1}: {2}", j, k, learningRate * DeltaH[(layers.Count - 1) - i][j] * sum);
                                Console.WriteLine("Updated Weight of neuron {0}, {1}: {2}", j, k, layers[i][j].weights[k]);
                            }
                            else
                            {
                                for (int l = 0; l < layers[i + 1].Count; l++)
                                {
                                    // Sums up all of the weights downstream from layer i, neuron j, weight k
                                    sum += layers[i + 1][l].weights[j] * DeltaH[((layers.Count - 1) - i) - 1][l];
                                }
                                // Calculates the delta for this weight on this neuron
                                DeltaH[(layers.Count - 1) - i].Add(sum * layers[i][j].Derivate());
                                // assigns said delta to the weight if the current layer isn't the input layer
                                layers[i][j].weights[k] -= learningRate * DeltaH[(layers.Count - 1) - i][j] * layers[i - 1][k].activation;
                                // Adjusts the bias
                                layers[i][j].bias -= learningRate * DeltaH[(layers.Count - 1) - i][j];
                                Console.WriteLine("dNet/dW of neuron {0} weight {1}: {2}", j, k, layers[i - 1][k].activation);
                                Console.WriteLine("Adjustment to output {0} weight {1}: {2}", j, k, learningRate * DeltaH[(layers.Count - 1) - i][j]);
                                Console.WriteLine("Updated Weight of neuron {0}, {1}: {2}", j, k, layers[i][j].weights[k]);
                            }
                        }
                    }
                }

                for (int i = 0; i < layers[0].Count; i++)
                {
                    // Performs back propagation on the input layer biases

                    double sum = 0;
                    for (int l = 0; l < layers[1].Count; l++)
                    {
                        // Sums up all of the weights downstream from layer i, neuron j, weight k
                        sum += layers[1][l].weights[i] * DeltaH[layers.Count - 2][l];
                    }

                    // Adjusts the bias
                    layers[0][i].bias -= sum * layers[0][i].Derivate();
                }
            }
                                                                        // Sets the inputs of each of the neurons to the corresponding input from the list
            layers[0][0].rawInput = inputs[0];
            layers[0][1].rawInput = inputs[1];

            nextLayerInput = new List<double>(2) { 0, 0 };
            for (int i = 0; i < 3; i++)
            {
                // Activates the current layer's neurons
                nextLayerInput[0] = layers[i][0].Activate();
                nextLayerInput[1] = layers[i][1].Activate();
                // Sets the inputs for the next layer, unless the current layer is the output layer
                if (i == 2)
                    break;
                // sets the next layer's first neuron's inputs
                layers[i + 1][0].inputs[0] = layers[i][0];
                layers[i + 1][0].inputs[1] = layers[i][1];
                // sets the next layer's second neuron's inputs
                layers[i + 1][1].inputs[0] = layers[i][0];
                layers[i + 1][1].inputs[1] = layers[i][1];
            }

            Console.WriteLine("The test output is: [{0}, {1}]", nextLayerInput[0], nextLayerInput[1]);

            Console.ReadKey();
        }

        public class Neuron
        {
            public List<double> weights;   // List of weights on the connections between this neuron and each of its inputs
            public List<Neuron> inputs;    // List of Neurons that are connected to this neuron
            public double rawInput;        // List of raw inputs to the neuron, used if the neuron is on the input layer
            public double bias;            // The bias for this neuron
            public double activation;      // The current activation of this neuron
            public double net;             // The net input to this neuron (before activation function)
            public bool inputLayer;        // Determines if the neuron is a part of the input layer

            public Neuron(double bias = 0)
            {
                // Initializes a neuron without having neurons attached to it, this is used for the input layer
                inputLayer = true;
                this.bias = bias;
            }

            public Neuron(List<Neuron> inputs, List<double> weights, double bias)
            {
                // Initializes a neuron that does have neurons attached to it
                inputLayer = false;
                this.inputs = inputs;
                this.weights = weights;
                this.bias = bias;
            }

            public double Activate()
            {
                // Calculates the net input to the neuron
                if (!inputLayer)
                {
                    net = bias;
                    for (int i = 0; i < inputs.Count; i++)
                        net += inputs[i].activation * weights[i];
                    activation = 1 / (1 + Math.Exp(-1 * net));
                }
                else
                {
                    net = rawInput + bias;
                    activation = net;
                }

                return activation;
            }

            public double Derivate()
            { 
                return activation * (1 - activation);
            }
        }
    }
}
