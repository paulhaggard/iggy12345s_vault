using NeuralNetworkFundamentals.Activation_Functions;
using NeuralNetworkFundamentals.Activation_Functions.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkFundamentals
{ 
    public class RecurrentNetwork : NeuralNetwork
    {
        // This will be used to implement recurrent networks, it will handle the linking of the layers back up to themselves.

        // With recurrent layers we need to be aware of the circular reference we are making between the output of the current neuron to itself
        // This means that in the training method, we need to activate not only the input layer, but each of the recurrent layers as well
        // This will allow the recurrent layers to activate, since the recurrent hidden layer acts as an input to the recurrent neuron.
        // so when the recurrent neuron attempts to fire for the first time it needs to be marked that the input for that hidden recurrent neuron has been collected.
        // Thus, why we need to activate them during the first iteration of training, after that, every time the recurrent neuron fires, it will cause the hidden layer to fire as well.

        private List<Tuple<List<Neuron>, int>> recurrentLayers;     // Holds a list of all of the hidden recurrent networks in the network
        private bool firstActivation;                               // Used to determine if the network has been through it's first feed-forward pass yet

        public RecurrentNetwork(List<LayerDesc> LayerInfo, List<ActivationFunction> defaultActivationFunction = null, List<ActivationParameters> Params = null,
            double learningRate = 0.5, double momentum = 0)
        {
            if (defaultActivationFunction == null)
            {
                defaultActivationFunction = new List<ActivationFunction>(LayerInfo.Count);
                for (int i = 0; i < LayerInfo.Count; i++)
                    defaultActivationFunction.Add(new Sigmoid());
            }

            if (Params == null)
            {
                Params = new List<ActivationParameters>(LayerInfo.Count);
                for (int i = 0; i < LayerInfo.Count; i++)
                    Params.Add(new SigmoidParams());
            }

            // initializes the list of links to the recurrent layers in the network, used for indexing later on.
            recurrentLayers = new List<Tuple<List<Neuron>, int>>(LayerInfo.Count);

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

                // With recurrent layers we need to be aware of the circular reference we are making between the output of the current neuron to itself
                // This means that in the training method, we need to activate not only the input layer, but each of the recurrent layers as well
                // This will allow the recurrent layers to activate, since the recurrent hidden layer acts as an input to the recurrent neuron.
                // so when the recurrent neuron attempts to fire for the first time it needs to be marked that the input for that hidden recurrent neuron has been collected.
                // Thus, why we need to activate them during the first iteration of training, after that, every time the recurrent neuron fires, it will cause the hidden layer to fire as well.

                if(LayerInfo[i].Type == LayerType.recurrent)
                {
                    // Initializes a temporary item to be added to the list later.
                    Tuple<List<Neuron>, int> tempRec = new Tuple<List<Neuron>, int>(new List<Neuron>(LayerInfo[i].Count), i);

                    for (int j = 0; j < tempRec.Item2; j++)
                        tempRec.Item1.Add(new Neuron(Layers[i],
                            defaultActivation: defaultActivationFunction[i],
                            defaultParameters: Params[i]));     // Generates a list of neurons that are linked to the output of the current layer

                    if(LayerInfo[i].HasOutputs)
                    {
                        // Determines where to send the output.
                        foreach(int index in LayerInfo[i].OutputIndex)
                        {
                            // Subscribes each output to each layer of neurons indicated in the Output Index list.
                            foreach (Neuron recNeuron in tempRec.Item1)
                                foreach (Neuron normNeuron in Layers[((index==-1) ? Layers.Count - 1 : index)])
                                    normNeuron.SubscribeToActivation(recNeuron);
                        }
                    }
                    else
                        // Subscribes each recurrent neuron to the original layer's neurons
                        foreach (Neuron recNeuron in tempRec.Item1)
                            foreach (Neuron normNeuron in Layers[Layers.Count - 1])
                                normNeuron.SubscribeToActivation(recNeuron);

                    recurrentLayers.Add(tempRec);   // Adds the current layer to the archive of recurrent layers.
                }
            }
        }

        public override void Train(int iterations, List<List<double>> sample_in, List<List<double>> sample_out, double errorThreshold = 0, bool Reset = false, int delay = 0, bool RxErrEvents = false)
        {
            if (Reset)
            {
                // TO DO: FIGURE OUT HOW TO ERASE THE MEMORY THAT IS IN THE NETWORK
            }

            base.Train(iterations, sample_in, sample_out, errorThreshold, Reset, delay, RxErrEvents);
        }

        public override void ForwardPropagate()
        {
            // This section of the override will take care of activating the hidden recurrent layers.
            if (firstActivation)
                foreach (Tuple<List<Neuron>, int> layer in recurrentLayers)
                    foreach (Neuron neuron in layer.Item1)
                        neuron.Activate();

            base.ForwardPropagate();
        }
    }
}
