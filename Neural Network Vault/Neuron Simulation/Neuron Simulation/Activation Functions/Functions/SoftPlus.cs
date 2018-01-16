using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation.Activation_Functions.Functions
{
    class Softplus : ActivationFunction
    {
        public override double Activate(double x, ActivationParameters Params)
        {
            return Math.Log(1 + Math.Exp(x));
        }

        public override double Derivate(double x, ActivationParameters Params)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }

    public class SoftplusParams : ActivationParameters
    {

    }
}
