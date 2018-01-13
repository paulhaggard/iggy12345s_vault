﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation.Activation_Functions.Functions
{
    class Gaussian : ActivationFunction
    {
        public override double Activate(double x, ActivationParameters Params)
        {
            return Math.Exp(-Math.Pow(x, 2));
        }

        public override double Derivate(double x, ActivationParameters Params)
        {
            return -2 * x * Activate(x, Params);
        }
    }

    public class GaussianParams : ActivationParameters
    {

    }
}
