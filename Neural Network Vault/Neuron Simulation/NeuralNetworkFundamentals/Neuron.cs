using NeuralNetworkFundamentals.Activation_Functions;
using NeuralNetworkFundamentals.Activation_Functions.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Troschuetz.Random;

namespace NeuralNetworkFundamentals
{ 
    public class Neuron
    {
        // This class is designed around the neural network example used here:
        // https://mattmazur.com/2015/03/17/a-step-by-step-backpropagation-example/

        /*
         * The inputWeights are the weights between the each neuron in the last layer, and the current neuron
         * the inputNeurons are the list of the each neuron that is in the layer previous to this one and should be updated between trainings.
         * inputLayer is used if this neuron is on the input layer
         * inputs is the array used if the neuron is on the input layer
         * inputs_collected determines when each neuron in the previous has fired and this neuron has received their outputs
         */

        // Delegates
        public delegate void ActivationEventHandler(object sender, ActivationEventArgs e);

        // Events
        public event ActivationEventHandler ActiveEvent;    // Triggered when this Neuron finishes calculating its activation value

        // Properties
        private double activation;          // This represents how activated the neuron is
        private List<double> weights;       // Weight to be passed to the next neuron
        private double net;                 // The net output of the neuron without being activated
        private double bias;                // The bias of the neuron
        private double error;               // Contains the error of the neuron
        private double prevError;           // previous error, used for momentum

        private bool inputLayer;            // Determines if the inputs to this neuron will be in the form of Neurons, or doubles
        private bool outputLayer;           // Determines if the outputs of this neuron are the final stage of the network, determines what kind of backpropagation to use

        private List<long> inputNeurons;    // List of IDs of the neurons that input into this neuron
        private List<double> inputs;        // Inputs into the Neuron if inputLayer is false
        private List<int> outputNeurons;    // Contains the indices of this neuron's weights in the next layer of neurons.

        private double rawInput;            // Inputs into the Neuron if inputLayer is true

        private bool[] inputs_collected;    // Specifies whether the inputs for the Neuron have been collected or not
        private Thread activationThread;    // Used to launch the activation event in a new thread as an asynchronous process.
        private bool isActivating;          // Flags when the neuron begins and finishes activating.

        private ActivationFunction defaultActivation;
        private ActivationParameters defaultParameters;

        private long id;                    // The unique Identifier that tells this Neuron apart form every other Neuron
        private static long NeuronCount;    // How many Neurons currently exist

        // Accessor Methods
        public double RawInput { get => rawInput; set => rawInput = value; }            // Sets the inputs to the Neuron
        public List<double> Weights { get => weights; set => weights = value; }   // Sets the initial weight value for the Neuron
        public double Activation { get => activation; set => activation = value; }  // The output of the Neuron
        public ActivationFunction DefaultActivation { get => defaultActivation; set => defaultActivation = value; }   // returns the default activation function class instance
        public ActivationParameters DefaultParameters { get => defaultParameters; set => defaultParameters = value; }
        public double Net { get => net; set => net = value; }
        public long ID { get => id; set => id = value; }
        public double Bias { get => bias; set => bias = value; }
        public double Threshold { get => -bias; set => bias = -value; }
        public double Error { get => error; set => error = value; }

        // Constructors
        // These all call the Setup Functions
        public Neuron(Neuron[] inputNeurons, List<double> weight = null, double bias = 0, bool outputLayer = false,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null)
        {
            // Creates a new neuron and links it to all of it's input Neurons
            Setup(inputNeurons.ToList(), weight, bias, outputLayer, defaultActivation, defaultParameters);
        }

        public Neuron(List<Neuron> inputNeurons, List<double> weight = null, double bias = 0, bool outputLayer = false,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null)
        {
            // Creates a new neuron and links it to all of it's input Neurons
            Setup(inputNeurons, weight, bias, outputLayer, defaultActivation, defaultParameters);
        }

        public Neuron(double bias = 0,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null, bool inputLayer = true)
        {
            // Specifies that this neuron is an input neuron
            Setup(bias, defaultActivation, defaultParameters, inputLayer);
        }

        // Performs the actual construction
        private void Setup(double bias = 0,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null, bool inputLayer = true)
        {
            activationThread = new Thread(OnActivation);    // Used to make the activation process asynchronous

            // Specifies that this neuron is an input neuron
            this.inputLayer = inputLayer;

            this.bias = bias;

            error = 0;  // initializes error value
            prevError = 0;

            this.defaultActivation = defaultActivation ?? new Sigmoid(); // default activation function
            this.defaultParameters = defaultParameters ?? new SigmoidParams(); // default activation parameters

            id = NeuronCount++;                         // assigns the Neuron ID and increments the count

            rawInput = 0;
        }

        private void Setup(List<Neuron> inputNeurons, List<double> weight = null, double bias = 0, bool outputLayer = false,
            ActivationFunction defaultActivation = null, ActivationParameters defaultParameters = null)
        {
            // Creates a new neuron and links it to all of it's input Neurons
            this.outputLayer = outputLayer;

            weights = weight ?? new List<double>(inputNeurons.Count());   // initial weight value
            if (weight == null)
                for (int i = 0; i < inputNeurons.Count; i++)
                    weights.Add(0);

            // Sets up the calling map for activating neurons and connecting with the previous layer
            this.inputNeurons = new List<long>(inputNeurons.Count);
            inputs = new List<double>(inputNeurons.Count);
            inputs_collected = new bool[inputNeurons.Count];

            for (int i = 0; i < inputs_collected.Length; i++)
            {
                inputs.Add(0);
                this.inputNeurons.Add(inputNeurons[i].ID);
                inputs_collected[i] = false;
            }

            Setup(bias, defaultActivation, defaultParameters, inputLayer);

            for (int i = 0; i < inputNeurons.Count; i++)
            {
                inputNeurons[i].ActiveEvent += OnActivate;  // Subscribes to the input Neuron's activation events
            }
        }

        // Methods
        public void OnActivate(object sender, ActivationEventArgs e)
        {
            // Figures out which Neuron fired this event, and then collects it's data.
            Neuron Sender = (Neuron)sender;
            if(inputNeurons.Contains(Sender.ID))
            {
                for (int i = 0; i < inputNeurons.Count; i++)
                {
                    if (Sender.id == inputNeurons[i])
                    {
                        // once the input Neuron's index is found, flag the boolean that corresponds to it and update it's value in the list
                        inputs_collected[i] = true;
                        inputs[i] = Sender.Activation;
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
                    Activate();
            }
        }

        public async virtual void Activate(ActivationFunction type = null, ActivationParameters Params = null)
        {
            // These are the various activation functions that I could find on wikipedia:
            // https://en.wikipedia.org/wiki/Activation_function

            // This function doesn't provide functionality for Softmax, or Maxout, obviously

            type = type ?? DefaultActivation;
            Params = Params ?? DefaultParameters;

            if (!inputLayer)
            {
                // Input layers don't have weights and activation functions, that's why they get an exclusive case
                Net = bias;
                for (int i = 0; i < (inputNeurons.Count); i++)
                    Net += (inputs[i]) * (weights[i]);
                for (int i = 0; i < inputs_collected.Length; i++)
                    inputs_collected[i] = false;
                activation = type.Activate(Net, Params);
            }
            else
            {
                Net = rawInput + bias;
                activation = net;
            }

            await Task.Run(new Action(OnActivation));

            //return Activation;
        }

        public void RandomizeWeights(NormalDistribution rnd)
        {
            // Randomizes the weights according to the random generator sent in.
            if (!inputLayer)
            {
                for (int i = 0; i < weights.Count; i++)
                {
                    weights[i] = rnd.NextDouble();
                }
            }
        }

        public void RandomizeBias(BinomialDistribution rnd)
        {
            // Randomizes the bias according to the random number generator sent in.
            bias = rnd.NextDouble();
        }

        public void AdjustValues(double momentum = 1, double learningRate = 1, double ExpectedOutput = 0, List < Neuron> nextLayerNeurons = null)
        {
            // Backpropagates the values of the weights and biases based on the error of this neuron
            if (!inputLayer)
            {
                for (int i = 0; i < weights.Count; i++)
                {
                    weights[i] += momentum * prevError + learningRate * error * inputs[i];
                }
            }
            bias += momentum * prevError + learningRate * error;
        }

        public void AssignError(double momentum = 1, double learningRate = 1,  double ExpectedOutput = 0, List<Neuron> nextLayerNeurons = null, bool AdjustValues = true)
        {
            prevError = error;
            error = -1 * defaultActivation.Derivate(activation, defaultParameters);
            if(nextLayerNeurons == null)
            {
                // Performs error calculation for output neurons
                if (outputLayer)
                {
                    error *= (ExpectedOutput - activation);
                }
                else
                    throw new InvalidOperationException("Invalid Neuron type!",
                        new Exception("Cannot calculate error of non-output layer neuron without the next layer's neurons."));
            }
            else
            {
                // Performs error calculation for non-output neurons
                if (outputNeurons == null)
                    PopulateOutputIndices(nextLayerNeurons);

                double sum = 0;
                for(int i = 0; i < nextLayerNeurons.Count; i++)
                {
                    sum += nextLayerNeurons[i].weights[outputNeurons[i]] * nextLayerNeurons[i].Error;
                }
                error *= sum;
            }

            void PopulateOutputIndices(List<Neuron> nextLayer)
            {
                outputNeurons = new List<int>(nextLayer.Count);
                for(int i = 0; i < nextLayer.Count; i++)
                {
                    if (nextLayer[i].inputNeurons.Contains(id))
                        for (int j = 0; j < nextLayer[i].inputNeurons.Count; j++)
                        {
                            if (nextLayer[i].inputNeurons[j] == id)
                            {
                                outputNeurons.Add(j);
                            }
                        }
                    else
                        throw new InvalidOperationException("Neuron not linked!",
                            new Exception("Cannot find this neuron's id in the next layer's neurons"));
                }
            }

            if (AdjustValues)
                this.AdjustValues(momentum, learningRate, ExpectedOutput, nextLayerNeurons);
        }

        protected virtual void OnActiveEvent(ActivationEventArgs e)
        {
            ActiveEvent?.Invoke(this, e);
        }

        public void OnActivation()
        {
            // A helper function used to call the OnActiveEvent event that requires no arguments.
            isActivating = true;
            OnActiveEvent(new ActivationEventArgs(activation, id));
            isActivating = false;
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
