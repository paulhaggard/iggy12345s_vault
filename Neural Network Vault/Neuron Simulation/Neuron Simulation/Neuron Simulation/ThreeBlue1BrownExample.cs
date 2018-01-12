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

        private Neuron[] n_inputs;
        private Neuron[] n_hidden1s;
        private Neuron[] n_hidden2s;
        private Neuron[] n_outputs;

        public ThreeBlue1BrownExample()
        {
            n_inputs = new Neuron[n_input];
            n_hidden1s = new Neuron[n_hidden1];
            n_hidden2s = new Neuron[n_hidden2];
            n_outputs = new Neuron[n_out];
        }

        public int Test()
        {
            // Tests the neural network by throwing a random image through it

        }
    }
}
