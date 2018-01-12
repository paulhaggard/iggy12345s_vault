using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation
{
    // This enum is a list of possible activation function types used by the real thing
    public enum Activations { Sigmoid = 0, Identity, BinaryStep, Tanh, ATan, Softsign, InverseRoot, ReLU, LeakyReLU,
    PReLU, RReLU, ELU, SELU, SReLU, ISRLU, APL, SoftPlus, BentIdentity, SoftExponential, Sinusoid, Sinc, Gaussian};

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
        private double input;       // The data that is input into the neuron
        private double activation;  // This represents how activated the neuron is
        private double weight;      // Weight assigned to this neuron

        public double Activate(Activations type = Activations.Sigmoid, ActivationParameters Params = new ActivationParameters())
        {
            // These are the various activation functions that I could find on wikipedia:
            // https://en.wikipedia.org/wiki/Activation_function

            // This function doesn't provide functionality for Softmax, or Maxout, obviously

            switch (type)
            {
                case (Activations.Sigmoid):
                    activation = 1 / (1 + Math.Exp(-input));
                    break;

                case Activations.Identity:
                    activation = input;
                    break;

                case Activations.BinaryStep:
                    activation = 0;
                    if (input >= 0)
                        activation = 1;
                    break;

                case Activations.Tanh:
                    activation = (2 / (1 + Math.Exp(-2 * input))) - 1;
                    break;

                case Activations.ATan:
                    activation = Math.Atan(input);
                    break;

                case Activations.Softsign:
                    activation = input / (1 + Math.Abs(input));
                    break;

                case Activations.InverseRoot:
                    // Inverse Square root unit
                    activation = input / Math.Sqrt(1 + Params.Alpha * Math.Pow(input, 2));
                    break;

                case Activations.ReLU:
                    // Rectified Linear Unit
                    activation = 0;
                    if (input >= 0)
                        activation = input;
                    break;

                case Activations.LeakyReLU:
                    // See ReLU
                    activation = 0.01 * input;
                    if (input >= 0)
                        activation = input;
                    break;

                case Activations.PReLU:
                    // Parametric
                    activation = Params.Alpha * input;
                    if (input >= 0)
                        activation = input;
                    break;

                case Activations.RReLU:
                    // Randomized leaky rectified linear unit
                    activation = Params.Alpha * input;
                    if (input >= 0)
                        activation = input;
                    break;

                case Activations.ELU:
                    // Exponential linear unit
                    activation = Params.Alpha * (Math.Exp(input) - 1);
                    if (input >= 0)
                        activation = input;
                    break;

                case Activations.SELU:
                    // Scaled ELU
                    activation = 1.0507 * 1.67326 * (Math.Exp(input) - 1);
                    if (input >= 0)
                        activation = 1.0507 * input;
                    break;

                case Activations.SReLU:
                    // S-shaped rectified linear activation unit
                    activation = Params.Tl + Params.Al * (input - Params.Tl);
                    if (input > Params.Tl && input < Params.Tr)
                        activation = input;
                    else if (input >= Params.Tr)
                        activation = Params.Tr + Params.Ar * (input - Params.Tr);
                    break;

                case Activations.ISRLU:
                    // Inverse Square root LU
                    activation = input / Math.Sqrt(1 + Params.Alpha * Math.Pow(input, 2));
                    if (input >= 0)
                        activation = input;
                    break;

                case Activations.APL:
                    // Adaptive Piecewise Linear
                    double total = 0;
                    for (int i = 1; i < Params.S; i++)
                        total += Math.Pow(Params.Alpha, Params.S) * Math.Max(0, -input + Math.Pow(Params.Beta, Params.S));
                    activation = Math.Max(0, input) + total;
                    break;

                case Activations.SoftPlus:
                    activation = Math.Log(1 + Math.Exp(input));
                    if (Params.Alpha == 0)
                        activation = input;
                    else if (Params.Alpha > 0)
                        activation = (Math.Exp(Params.Alpha * input) - 1) / Params.Alpha + Params.Alpha;
                    break;

                case Activations.BentIdentity:
                    activation = (Math.Sqrt(Math.Pow(input, 2) + 1) - 1) / 2 + input;
                    break;

                case Activations.SoftExponential:
                    activation = -(Math.Log(1 - Params.Alpha * (input + Params.Alpha)) / Params.Alpha);
                    break;

                case Activations.Sinusoid:
                    activation = Math.Sin(input);
                    break;

                case Activations.Sinc:
                    activation = 1;
                    if (input != 0)
                        activation = Math.Sin(input) / input;
                    break;

                case Activations.Gaussian:
                    activation = Math.Exp(-Math.Pow(input, 2));
                    break;

                default:
                    activation = 0;
                    break;
            }

            return activation;
        }
    }
}
