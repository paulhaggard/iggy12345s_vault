using System;
using System.Collections.Generic;
using NeuralNetworkFundamentals;
using static NeuralNetworkFundamentals.Neuron;

namespace Single_Neuron_Debugging_Testbench
{
    class Program
    {
        static bool activationFlag;

        static void Main(string[] args)
        {
            Random rnd = new Random();
            int numInputs = 2;
            List<double> weightInit = new List<double>() { rnd.NextDouble(), rnd.NextDouble() };
            // Creates the input to the test neuron
            List<Neuron> inputs = new List<Neuron>(numInputs);
            for(int i = 0; i < inputs.Capacity; i++)
            {
                inputs.Add(new Neuron());
            }
            // Creates an output layer neuron
            Neuron neuronTest = new Neuron(inputs,
                weightInit, 
                outputLayer: true);

            neuronTest.ActiveEvent += OnActivate;

            // Our testing sample values
            List<double> input = new List<double>() { rnd.NextDouble(), rnd.NextDouble()};
            double expectedOutput = rnd.NextDouble();
            char key;

            Console.WriteLine("For this test, the inputs are: {0} and {1}", input[0], input[1]);
            Console.WriteLine("The expected output will be: {0}", expectedOutput);
            Console.WriteLine("The initial weights are {0} and {1}", neuronTest.Weights[0], neuronTest.Weights[1]);
            Console.WriteLine("The initial bias is {0}", neuronTest.Bias);

            do
            {
                // Prepare the test
                activationFlag = true;
                for(int i = 0; i < inputs.Count; i++)
                {
                    // Activate each neuron in the input layer.
                    inputs[i].RawInput = input[i];
                    inputs[i].Activate();
                }
                while (activationFlag) ;    // Wait for the activation to finish.

                checkDelta();

                key = Console.ReadKey().KeyChar;

            } while (key != 's');

            void checkDelta()
            {
                // Checks and reports the error of the neuron to the console
                Console.WriteLine("The output of the neuron is {0} and should be {1}", neuronTest.Activation, expectedOutput);
                neuronTest.AssignDelta(1, 0.01, expectedOutput, AdjustValues: false);    // Calculates the derivatives of the neuron's error
                Console.WriteLine("The delta of the neuron was {0}", neuronTest.Delta);
                double prevBias = neuronTest.Bias;
                double prevWeight = neuronTest.Weights[0];
                neuronTest.AdjustValues(1, 0.01, expectedOutput);                        // Assigns the gradient to the weight and bias.
                Console.WriteLine("The Bias was changed by a factor of {0} to {1} from {2}", prevBias - neuronTest.Bias, neuronTest.Bias, prevBias);
                Console.WriteLine("The Weight was changed by a factor of {0} to {1} from {2}", prevWeight - neuronTest.Weights[0], neuronTest.Weights[0], prevWeight);
            }
        }

        static void OnActivate(object sender, ActivationEventArgs e)
        {
            //Neuron temp = (Neuron)sender;
            Console.WriteLine("The output for the neuron is {0} when the input is {1}", e.Activation, e.Input);
            activationFlag = false;
        }
    }
}
