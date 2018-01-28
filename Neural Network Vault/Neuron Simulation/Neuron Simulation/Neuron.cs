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
    public class Neuron
    {
        // This class is designed around the neural network example used here:
        // https://mattmazur.com/2015/03/17/a-step-by-step-backpropagation-example/

        /*
         * The inputWeights are the weights between the each neuron in the last layer, and the current neuron
         * the inputNeurons are the list of the each neuron that is in the layer previous to this one and should be updated between trainings.
         * raw_input is used if this neuron is on the input layer
         * inputs is the array used if the neuron is on the input layer
         * inputs_collected determines when each neuron in the previous has fired and this neuron has received their outputs
         */

        // Delegates
        public delegate void ActivationEventHandler(object sender, ActivationEventArgs e);

        // Events
        public event ActivationEventHandler ActiveEvent;    // Triggered when this Neuron finishes calculating its activation value

        // Properties
        private double activation;          // This represents how activated the neuron is
        private List<double> inputWeights;          // Weight to be passed to the next neuron
        private double bias_out;            // Bias to be passed to the next neuron
        private double net;                 // The net output of the neuron without being activated

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
        public List<double> Weight_in { get => inputWeights; set => inputWeights = value; }   // Sets the initial weight value for the Neuron
        public double Bias_in { get => bias_out; set => bias_out = value; }         // Sets the initial bias value for the Neuron
        public double Activation { get => activation; set => activation = value; }  // The output of the Neuron
        public ActivationFunction DefaultActivation { get => defaultActivation; set => defaultActivation = value; }   // returns the default activation function class instance
        public ActivationParameters DefaultParameters { get => defaultParameters; set => defaultParameters = value; }
        public double Net { get => net; set => net = value; }

        // Constructors
        public Neuron(ref Neuron[] inputNeurons, List<double> weight = null, double bias = 0,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null)
        {
            // Creates a new neuron and links it to all of it's input Neurons
            raw_input = false;

            inputWeights = weight ?? new List<double>(inputNeurons.Count());   // initial weight value
            bias_out = bias;                                                // initial bias value

            ID = NeuronCount++;                                             // assigns the Neuron ID and increments the count

            this.defaultActivation = defaultActivation??new Sigmoid(); // default activation function
            this.defaultParameters = defaultParameters??new SigmoidParams(); // default activation parameters (if you are using one that requires them)

            this.inputNeurons = inputNeurons.ToList();
            inputs_collected = new bool[inputNeurons.Length];
            for (int i = 0; i < inputs_collected.Length; i++)
                inputs_collected[i] = false;

            for (int i = 0; i < inputNeurons.Length; i++)
            {
                inputNeurons[i].ActiveEvent += OnActivate;  // Subscribes to the input Neuron's activation events
            }
        }

        public Neuron(ref List<Neuron> inputNeurons, List<double> weight = null, double bias = 0,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null)
        {
            // Creates a new neuron and links it to all of it's input Neurons
            raw_input = false;

            inputWeights = weight ?? new List<double>(inputNeurons.Count());   // initial weight value
            bias_out = bias;                            // initial bias value

            ID = NeuronCount++;                         // assigns the Neuron ID and increments the count

            this.defaultActivation = defaultActivation; // default activation function
            this.defaultParameters = defaultParameters; // default activation parameters (if you are using one that requires them)

            this.inputNeurons = inputNeurons.ToList();
            inputs_collected = new bool[inputNeurons.Count];
            for (int i = 0; i < inputs_collected.Length; i++)
                inputs_collected[i] = false;

            for (int i = 0; i < inputNeurons.Count; i++)
            {
                inputNeurons[i].ActiveEvent += OnActivate;  // Subscribes to the input Neuron's activation events
            }
        }

        public Neuron(int num_in = 1, List<double> weight = null, double bias = 0,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null)
        {
            raw_input = true;

            inputWeights = weight ?? new List<double>(num_in);   // initial weight value
            if (weight == null)
                for (int i = 0; i < num_in; i++)
                    inputWeights.Add(0);

            bias_out = bias;                            // initial bias value

            this.defaultActivation = defaultActivation; // default activation function
            this.defaultParameters = defaultParameters; // default activation parameters

            ID = NeuronCount++;                         // assigns the Neuron ID and increments the count

            Inputs = new double[num_in];
            for (int i = 0; i < num_in; i++)
                Inputs[i] = 0;
            inputs_collected = new bool[num_in];
            for (int i = 0; i < inputs_collected.Length; i++)
                inputs_collected[i] = false;

            for (int i = 0; i < num_in; i++)
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

            Net = bias_out;
            for (int i = 0; i < ((!raw_input) ? inputNeurons.Count : Inputs.Length); i++)
                Net += (!raw_input ? inputNeurons[i].Activation : Inputs[i]) * inputWeights[i];

            activation = type.Activate(Net, Params);

            OnActiveEvent(new ActivationEventArgs(Activation, ID));

            return Activation;
        }

        protected virtual void OnActiveEvent(ActivationEventArgs e)
        {
            ActiveEvent?.Invoke(this, e);
        }

        public class ActivationEventArgs : EventArgs
        {
            public double Activation { get; set; }
            public long ID { get; set; }

            public ActivationEventArgs(double activation, long ID)
            {
                Activation = activation;
                this.ID = ID;
            }
        }
    }
}
