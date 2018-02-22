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
            // Creates the input to the test neuron
            Neuron neuronTestInput = new Neuron();
            // Creates an output layer neuron
            Neuron neuronTest = new Neuron(new Neuron[] { neuronTestInput },
                new List<double>() { 0.1 }, 
                outputLayer: true);

            neuronTest.ActiveEvent += OnActivate;

            Random rnd = new Random();

            // Our testing sample values
            double input = 0.05;
            double expectedOutput = 0.6;
            char key;

            do
            {
                // Prepare the test
                activationFlag = true;
                neuronTestInput.RawInput = input;
                neuronTestInput.Activate(); // Activate the input
                while (activationFlag) ;    // Wait for the activation to finish.

                checkError();

                key = Console.ReadKey().KeyChar;

            } while (key != 's');

            void checkError()
            {
                // Checks and reports the error of the neuron to the console
                Console.WriteLine("The output of the neuron is {0} and should be {1}", neuronTest.Activation, expectedOutput);
                neuronTest.AssignError(1, 0.01, expectedOutput, AdjustValues: false);    // Calculates the derivatives of the neuron's error
                Console.WriteLine("The error of the neuron was {0}", neuronTest.Error);
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
