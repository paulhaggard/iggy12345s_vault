using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation
{
    class ThreeBlue1BrownExample
    {
        private int n_input = 748;      // Number of input neurons
        private int n_hidden1 = 16;     // Number of hidden neurons in the first hidden layer
        private int n_hidden2 = 16;     // Number of hidden neurons in the second hidden layer
        private int n_out = 10;         // Number of output neurons

        private int n_samples = 300;    // Number of random samples to generate to test the functionality

        private NeuralNetwork net;

        public ThreeBlue1BrownExample()
        {
            net = new NeuralNetwork(new List<int> { n_input, n_hidden1, n_hidden2, n_out });
        }

        public int Test()
        {
            // Tests the neural network by throwing a random image through it
            List<double[]> sampleData = new List<double[]>(n_samples);
        }
    }
}
