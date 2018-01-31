using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neuron_Simulation.Activation_Functions;

namespace Neuron_Simulation.Neurons
{
    class BiasNeuron : Neuron
    {
        private double bias;

        public double Bias { get => bias; set => bias = value; }

        // We just need the input Neurons to subscribe to their activation events.
        public BiasNeuron(double bias, List<Neuron> InputNeurons = null):base(ref InputNeurons)
        {
            this.bias = bias;
        }

        public override double Activate(ActivationFunction type = null, ActivationParameters Params = null)
        {
            // Notifies the next set of neurons that the bias is ready to use.
            OnActiveEvent(new ActivationEventArgs(bias, ID));
            return bias;
        }
    }
}
