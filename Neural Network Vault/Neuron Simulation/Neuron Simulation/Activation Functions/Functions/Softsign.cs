using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation.Activation_Functions.Functions
{
    class Softsign : ActivationFunction
    {
        public override double Activate(double x, ActivationParameters Params)
        {
            return x / (1 + Math.Abs(x));
        }

        public override double Derivate(double x, ActivationParameters Params)
        {
            return 1 / Math.Pow(1 + Math.Abs(x), 2);
        }
    }

    public class SoftsignParams : ActivationParameters
    {

    }
}
