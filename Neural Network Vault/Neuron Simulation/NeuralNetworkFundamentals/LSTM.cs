using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkFundamentals;
using NeuralNetworkFundamentals.Activation_Functions;
using NeuralNetworkFundamentals.Activation_Functions.Functions;

namespace NeuralNetworkFundamentals
{
    public class LSTM
    {
        private NeuralNetwork forgetGate;       // sigmoid forget gate
        private NeuralNetwork inputGate;        // sigmoid input gate
        private NeuralNetwork inputGate1;       // tanh input gate
        private NeuralNetwork outputGate;       // sigmoid output gate
        private List<double> recurrentMemory;   // The data that gets shifted back to the input.

        public LSTM(int inputSize, int memorySize = 1, int outputSize = 1)
        {
            NeuralNetwork temp = new NeuralNetwork(new List<int>() { 2 * inputSize, memorySize });
            forgetGate = temp;
            inputGate = temp;
            outputGate = new NeuralNetwork(new List<int>() { 2 * inputSize, outputSize });
            inputGate1 = new NeuralNetwork(new List<int>() { 2 * inputSize, memorySize },
                new List<ActivationFunction>() { new Tanh() },
                new List<ActivationParameters>() { new TanhParams() });
            recurrentMemory = new List<double>(memorySize);
            for (int i = 0; i < memorySize; i++)
                recurrentMemory.Add(0);
        }

        public virtual List<double> Activate(List<double> input)
        {
            // Loads and executes the sample in the lstm cell

            // Loads the sample into the different neural layers and forwards propagates them.
            forgetGate.LoadSample(input);
            forgetGate.ForwardPropagate();

            inputGate.LoadSample(input);
            inputGate.ForwardPropagate();

            inputGate1.LoadSample(input);
            inputGate1.ForwardPropagate();

            outputGate.LoadSample(input);
            outputGate.ForwardPropagate();

            // Updates the recurrent memory
            recurrentMemory = Add(
                Multiply(recurrentMemory, forgetGate.Output),
                Multiply(inputGate.Output, inputGate1.Output));



            forgetGate.Layers.Last();

        }

        private List<double> Multiply(List<double> a, List<double> b)
        {
            // Mutliplies two lists together, by element.

            if (a.Count != b.Count)
                throw new Exception("Cannot multiply two lists of different sizes");

            List<double> temp = new List<double>(a.Count);

            for (int i = 0; i < a.Count; i++)
                temp.Add(a[i] * b[i]);

            return temp;
        }

        private List<double> Add(List<double> a, List<double> b)
        {
            // Adds two lists together, by element.

            if (a.Count != b.Count)
                throw new Exception("Cannot add two lists of different sizes");

            List<double> temp = new List<double>(a.Count);

            for (int i = 0; i < a.Count; i++)
                temp.Add(a[i] + b[i]);

            return temp;
        }
    }
}
