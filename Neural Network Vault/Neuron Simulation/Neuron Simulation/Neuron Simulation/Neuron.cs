using Neuron_Simulation.Activation_Functions;
using Neuron_Simulation.Activation_Functions.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Neuron_Simulation.Activation_Functions.ActivationFunction;
using static Neuron_Simulation.Activation_Functions.Functions.Sigmoid;

namespace Neuron_Simulation
{ 
    class Neuron
    {
        // Delegates
        public delegate void ActivationEventHandler(object sender, ActivationEventArgs e);

        // Events
        public event ActivationEventHandler ActiveEvent;    // Triggered when this Neuron finishes calculating its activation value

        // Properties
        private double activation;          // This represents how activated the neuron is
        private double weight_out;          // Weight to be passed to the next neuron
        private double bias_out;            // Bias to be passed to the next neuron

        private bool raw_input;             // Determines if the inputs to this neuron will be in the form of Neurons, or doubles

        private List<Neuron> inputNeurons;      // Inputs into the Neuron if raw_input is false

        private double[] inputs;            // Inputs into the Neuron if raw_input is true

        private bool[] inputs_collected;    // Specifies whether the inputs for the Neuron have been collected or not

        private ActivationFunction defaultActivation;
        private ActivationParameters defaultParameters;

        private long ID;                    // The unique Identifier that tells this Neuron apart form every other Neuron
        private static long NeuronCount;    // How many Neurons currently exist

        // Accessor Methods
        public double[] Inputs { get => inputs; set => inputs = value; }            // Sets the inputs to the Neuron
        public double Weight_in { get => weight_out; set => weight_out = value; }   // Sets the initial weight value for the Neuron
        public double Bias_in { get => bias_out; set => bias_out = value; }         // Sets the initial bias value for the Neuron
        public double Activation { get => activation; set => activation = value; }  // The output of the Neuron
        internal ActivationFunction DefaultActivation { get => defaultActivation; set => defaultActivation = value; }   // returns the default activation function class instance
        public ActivationParameters DefaultParameters { get => defaultParameters; set => defaultParameters = value; }

        // Constructors
        public Neuron(ref Neuron[] inputNeurons, double weight = 0, double bias = 0,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null)
        {
            // Creates a new neuron and links it to all of it's input Neurons
            raw_input = false;

            weight_out = weight;                        // initial weight value
            bias_out = bias;                            // initial bias value

            ID = NeuronCount++;                         // assigns the Neuron ID and increments the count

            this.DefaultActivation = defaultActivation??new Sigmoid(); // default activation function
            this.DefaultParameters = defaultParameters??new SigmoidParams(); // default activation parameters (if you are using one that requires them)

            this.inputNeurons = inputNeurons.ToList();

            for(int i = 0; i < inputNeurons.Length; i++)
            {
                inputNeurons[i].ActiveEvent += OnActivate;  // Subscribes to the input Neuron's activation events
            }
        }

        public Neuron(ref List<Neuron> inputNeurons, double weight = 0, double bias = 0,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null)
        {
            // Creates a new neuron and links it to all of it's input Neurons
            raw_input = false;

            weight_out = weight;                        // initial weight value
            bias_out = bias;                            // initial bias value

            ID = NeuronCount++;                         // assigns the Neuron ID and increments the count

            this.DefaultActivation = defaultActivation; // default activation function
            this.DefaultParameters = defaultParameters; // default activation parameters (if you are using one that requires them)

            this.inputNeurons = inputNeurons.ToList();

            for (int i = 0; i < inputNeurons.Count; i++)
            {
                inputNeurons[i].ActiveEvent += OnActivate;  // Subscribes to the input Neuron's activation events
            }
        }

        public Neuron(int num_in = 1, double weight = 0, double bias = 0,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null)
        {
            raw_input = true;

            weight_out = weight;                        // initial weight value
            bias_out = bias;                            // initial bias value

            this.DefaultActivation = defaultActivation; // default activation function
            this.DefaultParameters = defaultParameters; // default activation parameters

            ID = NeuronCount++;                         // assigns the Neuron ID and increments the count

            Inputs = new double[num_in];

            for(int i = 0; i < num_in; i++)
            {
                Inputs[i] = 0;
            }
        }

        // Methods
        public void OnActivate(object sender, ActivationEventArgs e)
        {
            // Figures out which Neuron fired this event, and then collects it's data.
            Neuron Sender = (Neuron)sender;
            if(inputNeurons.Contains(Sender))
            {
                for (int i = 0; i < inputNeurons.Count; i++)
                {
                    if (Sender.ID == inputNeurons[i].ID)
                    {
                        // once the input Neuron's index is found, flag the boolean that corresponds to it and update it's value in the list
                        inputs_collected[i] = true;
                        inputNeurons[i] = Sender;
                    }
                }
                bool temp = true;
                foreach (bool item in inputs_collected)
                {
                    if (!item)
                    {
                        temp = false;
                        break;
                    }
                }
                if (temp)
                    Activate(DefaultActivation, DefaultParameters);
            }
        }

        public double Activate(ActivationFunction type = null, ActivationParameters Params = null)
        {
            // These are the various activation functions that I could find on wikipedia:
            // https://en.wikipedia.org/wiki/Activation_function

            // This function doesn't provide functionality for Softmax, or Maxout, obviously

            type = type ?? DefaultActivation;
            Params = Params ?? DefaultParameters;

            double temp = 0;
            for (int i = 0; i < inputNeurons.Count; i++)
                temp += (!raw_input ? inputNeurons[i].Activation : Inputs[i]) * inputNeurons[i].weight_out + inputNeurons[i].bias_out;

            activation = type.Activate(temp, Params);

            OnActiveEvent(new ActivationEventArgs(Activation, weight_out, bias_out));

            return Activation;
        }

        protected virtual void OnActiveEvent(ActivationEventArgs e)
        {
            ActiveEvent?.Invoke(this, e);
        }

        public class ActivationEventArgs : EventArgs
        {
            public double activation { get; set; }
            public double weight { get; set; }
            public double bias { get; set; }

            public ActivationEventArgs(double activation, double weight, double bias)
            {
                this.activation = activation;
                this.weight = weight;
                this.bias = bias;
            }
        }
    }
}
