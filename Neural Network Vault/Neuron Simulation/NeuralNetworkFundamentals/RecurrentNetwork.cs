using NeuralNetworkFundamentals.Activation_Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkFundamentals
{
    public enum LayerType { normal=0, recurrent, convolution}

    public class LayerDesc
    {
        // Determines what type of layer the layer of neurons is
        private int count;
        private LayerType type;

        public int Count { get => count; set => count = value; }
        public LayerType Type { get => type; set => type = value; }

        public LayerDesc(int count, LayerType type = LayerType.normal)
        {
            this.count = count;
            this.type = type;
        }
    }

    public class RecurrentNetwork : NeuralNetwork
    {
        // This will be used to implement recurrent networks, it will handle the linking of the layers back up to themselves.

        public RecurrentNetwork(List<LayerDesc> LayerInfo, List<ActivationFunction> defaultActivationFunction = null, List<ActivationParameters> Params = null,
            double learningRate = 0.5, double momentum = 0)
        {
            // Generates the layers of Neurons
            for (int i = 0; i < LayerInfo.Count; i++)
            {
                List<Neuron> temp = new List<Neuron>(LayerInfo[i].Count);
                if (i == 0)
                    for (int j = 0; j < LayerInfo[i].Count; j++)
                        temp.Add(new Neuron(defaultActivation: defaultActivationFunction[i], defaultParameters: Params[i]));        // Creates the input layer
                else
                {
                    List<Neuron> prev = Layers[i - 1];
                    for (int j = 0; j < LayerInfo[i].Count; j++)
                        temp.Add(new Neuron(prev,
                            defaultActivation: defaultActivationFunction[i],
                            defaultParameters: Params[i],
                            outputLayer: (i == LayerInfo.Count - 1)));  // Generates the rest of the layers
                }
                Layers.Add(temp);

                // ADD THE RECURRENT LAYER HERE!!!
            }
        }
    }
}
