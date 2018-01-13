using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation.Activation_Functions.Functions
{
    class Sinusoid : ActivationFunction
    {
        public override double Activate(double x, ActivationParameters Params)
        {
            return Math.Sin(x);
        }

        public override double Derivate(double x, ActivationParameters Params)
        {
            return Math.Cos(x);
        }
    }

    public class SinusoidParams : ActivationParameters
    {

    }
}
