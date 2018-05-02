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
        private NeuralNetwork ForgetGate;       //sigmoid forget gate
        private NeuralNetwork inputGate;
        private NeuralNetwork inputGate1;
        private NeuralNetwork outputGate;
        private List<double> recurrentMemory;

        public LSTM(int inputSize, int outputSize=1, int memorySize = 1)
        {
            NeuralNetwork temp = new NeuralNetwork(new List<int>() { 2 * inputSize, memorySize });
            ForgetGate = temp;
            inputGate = temp;
            outputGate = new NeuralNetwork(new List<int>() { 2 * inputSize, outputSize });
            inputGate1 = new NeuralNetwork(new List<int>() { 2 * inputSize, memorySize },
                new List<ActivationFunction>() { new Tanh() },
                new List<ActivationParameters>() { new TanhParams() });

            recurrentMemory = new List<double>(memorySize);
            for(int i=0; i< memorySize;i++)
            {
                recurrentMemory.Add(0);
            }

        }

        public virtual List<Double> Activate(List<double> input)
        {

            ForgetGate.LoadSample(input);
            ForgetGate.ForwardPropagate();

            inputGate.LoadSample(input);
            inputGate.ForwardPropagate();

            inputGate1.LoadSample(input);
            inputGate1.ForwardPropagate();

            outputGate.LoadSample(input);
            outputGate.ForwardPropagate();

            ForgetGate.Layers.Last();
            return recurrentMemory;
        }

        private List<double> multiply(List<double> a, List<double> b)
        {
            if (a.Count != b.Count)
                throw new Exception("Cannot multiply two lists of different lengths");
            List<double> temp = new List<double>(a.Count);

            for(int i=0; i<a.Count;i++)
            {
                temp.Add(a[i] * b[i]);

            }
            return temp;
        }
        private List<double> Add(List<double> a, List<double> b)
        {
            if (a.Count != b.Count)
                throw new Exception("Cannot Add two lists of different lengths");

            List<double> temp = new List<double>(a.Count);

            for (int i = 0; i < a.Count; i++)
            {
                temp.Add(a[i] + b[i]);

            }
            return temp;
        }
    }
}
