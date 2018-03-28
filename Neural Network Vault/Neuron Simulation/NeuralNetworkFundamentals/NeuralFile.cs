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
            this.nets = nets ?? new List<NeuralNetwork>();
            this.neurons = neurons ?? new List<Neuron>();
            this.other = other ?? new List<object>();
        }
    }

    public class NeuralFile
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
            FileContents temp = new FileContents(false);

            path = (path == "") ? defaultPath : path;

            // Loads the file
            XElement root = XElement.Load(path);

            // Checks if the file has any contents
            if (!root.HasElements)
                throw new Exception("Cannot load an empty file");

            // Sets the corresponding attribute to it's proper value

            // Retrieves the boolean values for what's in the file.
            temp.HasNet = Convert.ToBoolean(root.XPathSelectElement("hasNets").Value);
            temp.HasNeurons = Convert.ToBoolean(root.XPathSelectElement("hasNeurons").Value);
            temp.HasOther = Convert.ToBoolean(root.XPathSelectElement("hasOther").Value);

            if (temp.HasNet)
            {
                // Selects the list of all of the networks
                List<XElement> nets = root.XPathSelectElements("Nets/Network").ToList();

                foreach(XElement net in nets)
                {
                    temp.Nets.Add(new NeuralNetwork()
                    {
                        ID = (int)(object)net.XPathSelectElement("ID"),
                        Layers = (List<List<Neuron>>)(object)net.XPathSelectElement("Layers"),
                        LearningRate = (double)(object)net.XPathSelectElement("LearningRate"),
                        Momentum = (double)(object)net.XPathSelectElement("Momentum")
                    });
                }
            }

            // TODO: Add functionality for neurons and other stuff.

            return temp;
        }

        public virtual bool FileWrite(FileContents file, string path = "")
        {
            // Writes a neural network project to an xml file.
            path = (path == "") ? defaultPath : path;

            XElement rootTree = new XElement("Root",
                new XElement);

            rootTree.Add(WriteList<>)

            rootTree.Save((path == "") ? defaultPath : path);

            return true;
        }

        private XElement WriteList<T>(List<T> list)
        {
            XElement temp = new XElement("List");
            for (int i = 0; i < list.Count; i++)
                temp.Add(new XElement("Element" + i, list[i]));
            return temp;
        }
    }
}
