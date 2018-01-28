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
        private int n_input = 2;      // Number of input neurons
        private int n_hidden1 = 3;     // Number of hidden neurons in the first hidden layer
        private int n_hidden2 = 3;     // Number of hidden neurons in the second hidden layer
        private int n_out = 1;         // Number of output neurons

        private int n_samples = 4;    // Number of random samples to generate to test the functionality

        private int iterations = 3000; // Number of times to train the neural network.

        private NeuralNetwork net;

        // Constructor
        public ThreeBlue1BrownExample()
        {
            Net = new NeuralNetwork(new List<int> { n_input, n_hidden1, n_hidden2, n_out });
        }

        // Accessor Methods
        public NeuralNetwork Net { get => net; set => net = value; }
        public int N_samples { get => n_samples; set => n_samples = value; }
        public int Iterations { get => iterations; set => iterations = value; }

        public void Test()
        {
            // Tests the neural network by throwing a random image through it

            // Generates random samples
            List<List<double>> sampleDataInput = new List<List<double>>(N_samples);
            List<List<double>> sampleDataOutput = new List<List<double>>(N_samples);
            List<double> TestSample = new List<double>(n_input);
            NormalDistribution rndNorm = new NormalDistribution();

            // Creates training samples

            /*
            for(int i = 0; i < N_samples; i++)
            {
                sampleDataInput.Add(new List<double>(n_input));
                for (int j = 0; j < n_input; j++)
                    sampleDataInput[i].Add(rndNorm.NextDouble());

                sampleDataOutput.Add(new List<double>(n_out));
                for (int j = 0; j < n_out; j++)
                    sampleDataOutput[i].Add(rndNorm.NextDouble());
            }
            */

            sampleDataInput.Add(new List<double> { 0, 0 });
            sampleDataOutput.Add(new List<double> { 0 });
            sampleDataInput.Add(new List<double> { 0, 1 });
            sampleDataOutput.Add(new List<double> { 1 });
            sampleDataInput.Add(new List<double> { 1, 0 });
            sampleDataOutput.Add(new List<double> { 1 });
            sampleDataInput.Add(new List<double> { 1, 1 });
            sampleDataOutput.Add(new List<double> { 0 });

            // Creates a test sample
            for (int j = 0; j < n_input; j++)
                TestSample.Add(rndNorm.NextDouble());

            Console.WriteLine("Starting training...");

            // Trains the neural network
            Net.Train(Iterations, sampleDataInput, sampleDataOutput);

            // Executes the neural network
            //return Net.Calc(TestSample);
        }
    }
}
