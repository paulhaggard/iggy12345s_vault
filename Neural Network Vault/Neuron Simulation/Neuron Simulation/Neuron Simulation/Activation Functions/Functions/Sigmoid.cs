using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Neuron_Simulation.Activation_Functions.ActivationFunction;

namespace Neuron_Simulation.Activation_Functions.Functions
{
    class Sigmoid : ActivationFunction
    {
        public override double Activate(double x, ActivationParameters Params)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public override double Derivate(double x, ActivationParameters Params)
        {
            return Activate(x, Params) * (1 - Activate(x, Params));
        }
    }

    public class SigmoidParams : ActivationParameters
    {

    }
}
