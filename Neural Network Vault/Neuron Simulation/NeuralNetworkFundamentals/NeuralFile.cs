using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;

namespace NeuralNetworkFundamentals
{
    public struct FileContents
    {
        //  A structure for determining which objects are in the file, and what isn't

        // Properties
        private bool hasNet, hasNeurons, hasOther;  // Boolean determiners that specify if the file contains these things.
        private List<NeuralNetwork> nets;           // Contents of the file that are neural networks
        private List<Neuron> neurons;               // Contents of the file that are neurons
        private List<object> other;                 // Contents of the file that are others

        // Accessors
        public bool HasNet { get => hasNet; set => hasNet = value; }
        public bool HasNeurons { get => hasNeurons; set => hasNeurons = value; }
        public bool HasOther { get => hasOther; set => hasOther = value; }
        public List<NeuralNetwork> Nets { get => nets; set => nets = value; }
        public List<Neuron> Neurons { get => neurons; set => neurons = value; }
        public List<object> Other { get => other; set => other = value; }

        // Constructor
        public FileContents(bool hasNet = false, bool hasNeurons = false, bool hasOther = false, 
            List<NeuralNetwork> nets = null, List<Neuron> neurons = null, List<object> other = null)
        {
            this.hasNet = hasNet;
            this.hasNeurons = hasNeurons;
            this.hasOther = hasOther;
            this.nets = nets;
            this.neurons = neurons;
            this.other = other;
        }
    }

    class NeuralFile
    {
        private string defaultPath;

        public string DefaultPath { get => defaultPath; set => defaultPath = value; }

        public NeuralFile(string defaultPath = "")
        {
            this.defaultPath = defaultPath;
        }

        // A class for reading in the weights and biases into a network from a file.
        public virtual FileContents FileRead(string path = "")
        {
            // Reads an xml file for networks, custom neurons, and other things...
            FileContents temp = new FileContents();

            path = (path == "") ? defaultPath : path;

            // Loads the file
            XElement root = XElement.Load(path);

            // Checks if the file has any contents
            if (!root.HasElements)
                throw new Exception("Cannot load an empty file");

            // Sets the corresponding attribute to it's proper value
            root.XPathSelectElement

            return temp;
        }

        public virtual bool FileWrite(FileContents file, string path = "")
        {
            // Writes a neural network project to an xml file.
            path = (path == "") ? defaultPath : path;

            XElement rootTree = new XElement("Root",
                new XElement("hasNets", file.HasNet),
                new XElement("hasNeurons", file.HasNeurons),
                new XElement("hasOther", file.HasOther));

            if(file.HasNet)
            {
                // Adds all of the included networks into the xml tree.
                XElement netTree = new XElement("Nets",
                    new XAttribute("NetCount", NeuralNetwork.NetCount));
                foreach(NeuralNetwork net in file.Nets)
                {
                    netTree.Add(new XElement("Network",
                        new XAttribute("ID", net.ID),
                        new XElement("Layers", net.Layers),
                        new XElement("Weights", net.Weights),
                        new XElement("Biases", net.Biases),
                        new XElement("LearningRate", net.LearningRate),
                        new XElement("Momentum", net.Momentum)));
                }

                // Adds the sub tree back to the original.
                rootTree.Add(netTree);
            }

            if(file.HasNeurons)
            {
                // Adds all of the included neurons into the xml tree.
                XElement neuronTree = new XElement("Neurons",
                    new XAttribute("NeuronCount", Neuron.Count));
                foreach(Neuron neuron in file.Neurons)
                {
                    neuronTree.Add(new XElement("Neuron",
                        new XAttribute("ID", neuron.ID),
                        new XAttribute("InputLayer", neuron.InputLayer),
                        new XAttribute("OutputLayer", neuron.OutputLayer),
                        new XElement("Network", neuron.Net),
                        new XElement("previousWeights", neuron.PrevWeights),
                        new XElement("previousDelta", neuron.PrevDelta),
                        new XElement("Weights", neuron.Weights),
                        new XElement("Bias", neuron.Bias),
                        new XElement("Activation", neuron.Activation),
                        new XElement("DefaultActivation",
                            new XAttribute("parameters", neuron.DefaultParameters),
                            neuron.DefaultActivation),
                        new XElement("Threshold", neuron.Threshold),
                        new XElement("Delta", neuron.Delta),
                        new XElement("RawInput", neuron.RawInput)));
                }
                rootTree.Add(neuronTree);
            }

            if(file.HasOther)
            {
                // Adds anything else to the xml tree
                XElement otherTree = new XElement("Others");
                for(int i = 0; i < file.Other.Count; i++)
                {
                    otherTree.Add(new XElement("Other",
                        new XAttribute("ID", i),
                        file.Other[i]));
                }
                rootTree.Add(otherTree);
            }

            // TODO: Write the tree to a file.
            // START HERE: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/serializing-xml-trees

            rootTree.Save((path == "") ? defaultPath : path);

            return true;
        }
    }
}
