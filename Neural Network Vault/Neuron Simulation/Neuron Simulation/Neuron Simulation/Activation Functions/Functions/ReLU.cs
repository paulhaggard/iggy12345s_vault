using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation.Activation_Functions.Functions
{
    class ReLU : ActivationFunction
    {
        // Rectified Linear Unit
        public override double Activate(double x, ActivationParameters Params)
        {
            return (x < 0) ? 0 : x;
        }

        public override double Derivate(double x, ActivationParameters Params)
        {
            return (x < 0) ? 0 : 1;
        }
    }

    public class ReLUParams : ActivationParameters
    {
    }
}
