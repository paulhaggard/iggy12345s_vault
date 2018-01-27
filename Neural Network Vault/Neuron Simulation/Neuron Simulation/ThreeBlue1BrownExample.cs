using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;

namespace Neuron_Simulation
{
    class ThreeBlue1BrownExample
    {
        private int n_input = 748;      // Number of input neurons
        private int n_hidden1 = 16;     // Number of hidden neurons in the first hidden layer
        private int n_hidden2 = 16;     // Number of hidden neurons in the second hidden layer
        private int n_out = 10;         // Number of output neurons

        private int n_samples = 300;    // Number of random samples to generate to test the functionality

        private int iterations = 10000; // Number of times to train the neural network.

        private NeuralNetwork net;

        public ThreeBlue1BrownExample()
        {
            net = new NeuralNetwork(new List<int> { n_input, n_hidden1, n_hidden2, n_out });
        }

        public int Test()
        {
            // Tests the neural network by throwing a random image through it

            // Generates random samples
            List<List<double>> sampleDataInput = new List<List<double>>(n_samples);
            List<List<double>> sampleDataOutput = new List<List<double>>(n_samples);
            List<double> TestSample = new List<double>(n_input);
            NormalDistribution rndNorm = new NormalDistribution();

            // Creates training samples
            for(int i = 0; i < n_samples; i++)
            {
                sampleDataInput.Add(new List<double>(n_input));
                for (int j = 0; j < n_input; j++)
                    sampleDataInput[i].Add(rndNorm.NextDouble());

                sampleDataOutput.Add(new List<double>(n_out));
                for (int j = 0; j < n_out; j++)
                    sampleDataOutput[i].Add(rndNorm.NextDouble());
            }

            // Creates a test sample
            for (int j = 0; j < n_input; j++)
                TestSample.Add(rndNorm.NextDouble());

            // Trains the neural network
            net.Train(iterations, sampleDataInput, sampleDataOutput);

            // Executes the neural network
            return net.Calc(TestSample);
        }
    }
}
