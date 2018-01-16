using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation.Activation_Functions.Functions
{
    class LeakyReLU : ActivationFunction
    {
        // Leaky ReLU
        public override double Activate(double x, ActivationParameters Params)
        {
            return (x < 0) ? 0.01 * x : x;
        }

        public override double Derivate(double x, ActivationParameters Params)
        {
            return (x < 0) ? 0.01 : 1;
        }
    }

    public class LeakyReLUParams : ActivationParameters
    {
    }
}
