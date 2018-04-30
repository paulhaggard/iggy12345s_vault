using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkFundamentals;

namespace RecurrentNetworkTestbench
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Setting up the test.");
            RecurrentNetwork net = new RecurrentNetwork(
                new List<LayerDesc>() { new LayerDesc(2), new LayerDesc(3, LayerType.recurrent), new LayerDesc(1) });

            net.TrainingUpdateEvent += OnTrainingUpdate;

            int iterations = 5000;

            List<List<double>> sampleIn = new List<List<double>>()
            {
                new List<double>() { 0, 0 },
                new List<double>() { 0, 1 },
                new List<double>() { 1, 0 },
                new List<double>() { 1, 1 }
            };

            List<List<double>> sampleOut = new List<List<double>>()
            {
                new List<double>(){0 },
                new List<double>(){1 },
                new List<double>(){1 },
                new List<double>(){0 }
            };

            Console.WriteLine("Setup complete!");
            Console.WriteLine("Running tests...");

            net.Train(iterations, sampleIn, sampleOut);

            Console.ReadKey();
        }

        static void OnTrainingUpdate(object sender, TrainingUpdateEventArgs e)
        {
            Console.WriteLine("Received update from network at iteration {0}", e.Iteration);
        }
    }
}
