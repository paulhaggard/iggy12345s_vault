using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation
{
    // This enum is a list of possible activation function types used by the real thing
    public enum Activations { Sigmoid = 0, Identity, BinaryStep, Tanh, ATan, Softsign, InverseRoot, ReLU, LeakyReLU,
    PReLU, RReLU, ELU, SELU, SReLU, ISRLU, APL, SoftPlus, BentIdentity, SoftExponential, Sinusoid, Sinc, Gaussian, Default};

    public struct ActivationParameters
    {
        // The list of all possible activation parameters used in the activation functions
        private double alpha;
        private double beta;
        private double lambda;
        private double tl;
        private double tr;
        private double al;
        private double ar;
        private double s;

        public ActivationParameters(double alpha = 0, double beta = 0, double lambda = 0, double tl = 0, double tr = 0, double al = 0, double ar = 0, double s = 0)
        {
            this.alpha = alpha;
            this.beta = beta;
            this.lambda = lambda;
            this.tl = tl;
            this.tr = tr;
            this.al = al;
            this.ar = ar;
            this.s = s;
        }

        public double Alpha { get => alpha; set => alpha = value; }
        public double Lambda { get => lambda; set => lambda = value; }
        public double Tl { get => tl; set => tl = value; }
        public double Tr { get => tr; set => tr = value; }
        public double Al { get => al; set => al = value; }
        public double Ar { get => ar; set => ar = value; }
        public double S { get => s; set => s = value; }
        public double Beta { get => beta; set => beta = value; }
    }

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

        private Activations defaultActivation;
        private ActivationParameters defaultParameters;

        private long ID;                    // The unique Identifier that tells this Neuron apart form every other Neuron
        private static long NeuronCount;    // How many Neurons currently exist

        // Accessor Methods
        public double[] Inputs { get => inputs; set => inputs = value; }            // Sets the inputs to the Neuron
        public double Weight_in { get => weight_out; set => weight_out = value; }   // Sets the initial weight value for the Neuron
        public double Bias_in { get => bias_out; set => bias_out = value; }         // Sets the initial bias value for the Neuron
        public double Activation { get => activation; set => activation = value; }  // The output of the Neuron

        // Constructors
        public Neuron(ref Neuron[] inputNeurons, double weight = 0, double bias = 0,
            Activations defaultActivation = Activations.Sigmoid, ActivationParameters defaultParameters = new ActivationParameters())
        {
            // Creates a new neuron and links it to all of it's input Neurons
            raw_input = false;

            weight_out = weight;                        // initial weight value
            bias_out = bias;                            // initial bias value

            ID = NeuronCount++;                         // assigns the Neuron ID and increments the count

            this.defaultActivation = defaultActivation; // default activation function
            this.defaultParameters = defaultParameters; // default activation parameters (if you are using one that requires them)

            this.inputNeurons = inputNeurons.ToList();

            for(int i = 0; i < inputNeurons.Length; i++)
            {
                inputNeurons[i].ActiveEvent += OnActivate;  // Subscribes to the input Neuron's activation events
            }
        }

        public Neuron(ref List<Neuron> inputNeurons, double weight = 0, double bias = 0,
            Activations defaultActivation = Activations.Sigmoid, ActivationParameters defaultParameters = new ActivationParameters())
        {
            // Creates a new neuron and links it to all of it's input Neurons
            raw_input = false;

            weight_out = weight;                        // initial weight value
            bias_out = bias;                            // initial bias value

            ID = NeuronCount++;                         // assigns the Neuron ID and increments the count

            this.defaultActivation = defaultActivation; // default activation function
            this.defaultParameters = defaultParameters; // default activation parameters (if you are using one that requires them)

            this.inputNeurons = inputNeurons.ToList();

            for (int i = 0; i < inputNeurons.Count; i++)
            {
                inputNeurons[i].ActiveEvent += OnActivate;  // Subscribes to the input Neuron's activation events
            }
        }

        public Neuron(int num_in = 1, double weight = 0, double bias = 0,
            Activations defaultActivation = Activations.Sigmoid, ActivationParameters defaultParameters = new ActivationParameters())
        {
            raw_input = true;

            weight_out = weight;                        // initial weight value
            bias_out = bias;                            // initial bias value

            this.defaultActivation = defaultActivation; // default activation function
            this.defaultParameters = defaultParameters; // default activation parameters

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
                    Activate(defaultActivation, defaultParameters);
            }
        }

        public double Activate(Activations type = Activations.Default, ActivationParameters Params = new ActivationParameters())
        {
            // These are the various activation functions that I could find on wikipedia:
            // https://en.wikipedia.org/wiki/Activation_function

            // This function doesn't provide functionality for Softmax, or Maxout, obviously

            if (type == Activations.Default)
                type = defaultActivation;

            double temp = 0;
            for (int i = 0; i < inputNeurons.Count; i++)
                temp += (raw_input ? inputNeurons[i].Activation : Inputs[i]) * inputNeurons[i].weight_out + inputNeurons[i].bias_out;

            switch (type)
            {
                case (Activations.Sigmoid):
                    Activation = 1 / (1 + Math.Exp(-temp));
                    break;

                case Activations.Identity:
                    Activation = temp;
                    break;

                case Activations.BinaryStep:
                    Activation = 0;
                    if (temp >= 0)
                        Activation = 1;
                    break;

                case Activations.Tanh:
                    Activation = (2 / (1 + Math.Exp(-2 * temp))) - 1;
                    break;

                case Activations.ATan:
                    Activation = Math.Atan(temp);
                    break;

                case Activations.Softsign:
                    Activation = temp / (1 + Math.Abs(temp));
                    break;

                case Activations.InverseRoot:
                    // Inverse Square root unit
                    Activation = temp / Math.Sqrt(1 + Params.Alpha * Math.Pow(temp, 2));
                    break;

                case Activations.ReLU:
                    // Rectified Linear Unit
                    Activation = 0;
                    if (temp >= 0)
                        Activation = temp;
                    break;

                case Activations.LeakyReLU:
                    // See ReLU
                    Activation = 0.01 * temp;
                    if (temp >= 0)
                        Activation = temp;
                    break;

                case Activations.PReLU:
                    // Parametric
                    Activation = Params.Alpha * temp;
                    if (temp >= 0)
                        Activation = temp;
                    break;

                case Activations.RReLU:
                    // Randomized leaky rectified linear unit
                    Activation = Params.Alpha * temp;
                    if (temp >= 0)
                        Activation = temp;
                    break;

                case Activations.ELU:
                    // Exponential linear unit
                    Activation = Params.Alpha * (Math.Exp(temp) - 1);
                    if (temp >= 0)
                        Activation = temp;
                    break;

                case Activations.SELU:
                    // Scaled ELU
                    Activation = 1.0507 * 1.67326 * (Math.Exp(temp) - 1);
                    if (temp >= 0)
                        Activation = 1.0507 * temp;
                    break;

                case Activations.SReLU:
                    // S-shaped rectified linear activation unit
                    Activation = Params.Tl + Params.Al * (temp - Params.Tl);
                    if (temp > Params.Tl && temp < Params.Tr)
                        Activation = temp;
                    else if (temp >= Params.Tr)
                        Activation = Params.Tr + Params.Ar * (temp - Params.Tr);
                    break;

                case Activations.ISRLU:
                    // Inverse Square root LU
                    Activation = temp / Math.Sqrt(1 + Params.Alpha * Math.Pow(temp, 2));
                    if (temp >= 0)
                        Activation = temp;
                    break;

                case Activations.APL:
                    // Adaptive Piecewise Linear
                    double total = 0;
                    for (int i = 1; i < Params.S; i++)
                        total += Math.Pow(Params.Alpha, Params.S) * Math.Max(0, -temp + Math.Pow(Params.Beta, Params.S));
                    Activation = Math.Max(0, temp) + total;
                    break;

                case Activations.SoftPlus:
                    Activation = Math.Log(1 + Math.Exp(temp));
                    if (Params.Alpha == 0)
                        Activation = temp;
                    else if (Params.Alpha > 0)
                        Activation = (Math.Exp(Params.Alpha * temp) - 1) / Params.Alpha + Params.Alpha;
                    break;

                case Activations.BentIdentity:
                    Activation = (Math.Sqrt(Math.Pow(temp, 2) + 1) - 1) / 2 + temp;
                    break;

                case Activations.SoftExponential:
                    Activation = -(Math.Log(1 - Params.Alpha * (temp + Params.Alpha)) / Params.Alpha);
                    break;

                case Activations.Sinusoid:
                    Activation = Math.Sin(temp);
                    break;

                case Activations.Sinc:
                    Activation = 1;
                    if (temp != 0)
                        Activation = Math.Sin(temp) / temp;
                    break;

                case Activations.Gaussian:
                    Activation = Math.Exp(-Math.Pow(temp, 2));
                    break;

                default:
                    Activation = 0;
                    break;
            }

            OnActiveEvent(new ActivationEventArgs(Activation, weight_out, bias_out));

            return Activation;
        }

        protected virtual void OnActiveEvent(ActivationEventArgs e)
        {
            ActivationEventHandler handler = ActiveEvent;
            if(handler != null)
            {
                handler(this, e);
            }
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
