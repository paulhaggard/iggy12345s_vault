using Neuron_Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;

namespace Debugging_Console_App
{ 
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Setting up test...");
            Neuron Test = new Neuron();

            NormalDistribution rndNorm = new NormalDistribution
            {
                Sigma = 0.05,
                Mu = 0
            };

            BinomialDistribution rndBin = new BinomialDistribution();

            Random rnd = new Random();

            double sample;

            NeuralNetwork net = new NeuralNetwork(new List<int> { 1, 1, 1 });

            List<double> samples_in = new List<double> { 1, 0 };
            List<double> samples_out = new List<double> { 0, 1 };

            long iterations = 500;


            net.Subscribe();

            Console.WriteLine("Test Setup Complete!\n");

            for (int i = 0; i < net.Layers.Count; i++)
            {

                Console.WriteLine("The Neurons {0}:", net.Layers[i][0].ID);

                net.Layers[i][0].Bias_in = rndBin.NextDouble();
                net.Layers[i][0].Weight_in = new List<double> { rndNorm.NextDouble() };

                Console.WriteLine("Weight: {0}", net.Layers[i][0].Weight_in[0]);
                Console.WriteLine("Bias: {0}", net.Layers[i][0].Bias_in);
            }

            Console.WriteLine("Training Samples... For {0} iterations", iterations);

            for(long i = 0; i < iterations; i++)
            {
                for (int j = 0; j < samples_in.Count; j++)
                {
                    Console.WriteLine("Iteration {0}, sample {1}: The data will be {2} in, and {3} out.", i, j, samples_in[j], samples_out[j]);
                    net.LoadSample(new List<double> { samples_in[0] });
                    Console.WriteLine("Loaded the sample\n");
                    net.ForwardPropagate();
                    Console.WriteLine("Propagated forward, the output was {0}", net.Layers.Last()[0].Activation);
                    double error = net.BackPropagate(new List<double> { samples_out[0] });

                    Console.WriteLine("\nAfter back-propagating the Neuron status' are:\n");
                    for (int k = 0; k < net.Layers.Count; k++)
                    {

                        Console.WriteLine("The Neurons {0}:", net.Layers[k][0].ID);

                        net.Layers[k][0].Bias_in = rndBin.NextDouble();
                        net.Layers[k][0].Weight_in = new List<double> { rndNorm.NextDouble() };

                        Console.WriteLine("Weight: {0}", net.Layers[k][0].Weight_in[0]);
                        Console.WriteLine("Bias: {0}", net.Layers[k][0].Bias_in);
                        Console.WriteLine("Activation: {0}", net.Layers[k][0].Activation);
                    }

                    Console.WriteLine("The error was: {0}\n", error);
                }
            }


            while (true)
            {

                sample = rndBin.NextDouble();

                Test.Inputs = new double[] { sample };

                Console.WriteLine("Feeding the Neuron {0} yielded {1}.", sample, Test.Activate());
                Console.WriteLine("Deriving the output {0} yields {1}", Test.Activation, Test.DefaultActivation.Derivate(Test.Activation, Test.DefaultParameters));

                Console.ReadKey();

                Console.WriteLine("Testing to train an inverter...");

            }

        }
    }
}
