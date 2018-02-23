using System;
using System.Collections.Generic;
using NeuralNetworkFundamentals;
using static NeuralNetworkFundamentals.Neuron;


namespace Single_Neuron_Debugging_Testbench
{
    class Program
    {
        static bool activationFlag;
        static long outputID;

        static void Main(string[] args)
        {
            Random rnd = new Random();
            int numInputs = 1;
            double learningRate = 0.5;
            List<double> weightInit1 = new List<double>() { rnd.NextDouble() };
            List<double> weightInit2 = new List<double>() { rnd.NextDouble() };
            // Creates the input to the test neuron
            List<Neuron> inputs = new List<Neuron>(numInputs);
            for (int i = 0; i < inputs.Capacity; i++)
            {
                inputs.Add(new Neuron());
            }
            // Creates an output layer neuron
            Neuron neuronTest1 = new Neuron(inputs,
                weightInit1);
            Neuron neuronTest2 = new Neuron(new List<Neuron>() { neuronTest1 },
                weightInit2,
                outputLayer: true);
            //Neuron neuronTest = new Neuron(inputs, outputLayer: true);
            //neuronTest.RandomizeWeights();

            outputID = neuronTest2.ID;
            neuronTest2.ActiveEvent += OnActivate;
            neuronTest1.ActiveEvent += OnActivate;

            // Our testing sample values
            List<List<double>> input = new List<List<double>>() {
                new List<double>() { rnd.NextDouble() } };
            //new List<double>() { 1, 0 } };
            List<double> expectedOutput = new List<double>() { rnd.NextDouble() };
            char key;

            Console.WriteLine("For this test, the inputs is: {0}", input[0][0]);//, input[0][1]);
            //Console.WriteLine("The second set of inputs are: {0} and {1}", input[1][0], input[1][1]);
            Console.WriteLine("The expected output will be: {0}", expectedOutput[0]);//, expectedOutput[1]);
            Console.WriteLine("The initial weights are {0} and {1}", neuronTest1.Weights[0], neuronTest2.Weights[0]);
            Console.WriteLine("The initial bias is {0} and {1}\n", neuronTest1.Bias, neuronTest2.Bias);
            Console.WriteLine("The learning rate is {0}", learningRate);
            Console.WriteLine("The momentum is 1");

            long iteration = 0;
            do
            {
                Console.WriteLine("\nIteration {0}", iteration++);
                // Prepare the test
                for (int epoch = 0; epoch < input.Count; epoch++)
                {
                    Console.WriteLine("For this sample, the input is: {0}", input[epoch][0]);//, input[epoch][1]);
                    activationFlag = true;
                    for (int i = 0; i < inputs.Count; i++)
                    {
                        // Activate each neuron in the input layer.
                        inputs[i].RawInput = input[epoch][i];
                        inputs[i].Activate();
                    }
                    while (activationFlag) ;    // Wait for the activation to finish.

                    // Checks and reports the error of the neuron to the console
                    Console.WriteLine("The output of the  output neuron is {0} and should be {1}", neuronTest2.Activation, expectedOutput[epoch]);
                    neuronTest2.AssignDelta(1, learningRate, expectedOutput[epoch], AdjustValues: false);    // Calculates the derivatives of the neuron's error
                    Console.WriteLine("The delta of the first neuron was {0}", neuronTest2.Delta);
                    double prevBias2 = neuronTest2.Bias;
                    double prevWeight2 = neuronTest2.Weights[0];
                    neuronTest2.AdjustValues(1, learningRate, expectedOutput[epoch]);                        // Assigns the gradient to the weight and bias.
                    Console.WriteLine("The Bias was changed by a factor of {0} to {1} from {2}", prevBias2 - neuronTest2.Bias, neuronTest2.Bias, prevBias2);
                    Console.WriteLine("The first Weight was changed by a factor of {0} to {1} from {2}", prevWeight2 - neuronTest2.Weights[0], neuronTest2.Weights[0], prevWeight2);
                    //Console.WriteLine("The first Weight was changed by a factor of {0} to {1} from {2}", prevWeight2 - neuronTest2.Weights[1], neuronTest2.Weights[1], prevWeight2);

                    Console.WriteLine("The output of the  hidden neuron is {0} and should be {1}", neuronTest1.Activation, expectedOutput[epoch]);
                    neuronTest2.AssignDelta(1, learningRate, expectedOutput[epoch], AdjustValues: false);    // Calculates the derivatives of the neuron's error
                    Console.WriteLine("The delta of the second neuron was {0}", neuronTest1.Delta);
                    double prevBias1 = neuronTest1.Bias;
                    double prevWeight1 = neuronTest1.Weights[0];
                    neuronTest2.AdjustValues(1, learningRate, expectedOutput[epoch]);                        // Assigns the gradient to the weight and bias.
                    Console.WriteLine("The Bias was changed by a factor of {0} to {1} from {2}", prevBias1 - neuronTest1.Bias, neuronTest1.Bias, prevBias1);
                    Console.WriteLine("The first Weight was changed by a factor of {0} to {1} from {2}", prevWeight1 - neuronTest1.Weights[0], neuronTest1.Weights[0], prevWeight1);
                    //Console.WriteLine("The first Weight was changed by a factor of {0} to {1} from {2}", prevWeight2 - neuronTest1.Weights[1], neuronTest1.Weights[1], prevWeight2);
                }

                key = Console.ReadKey().KeyChar;

            } while (key != 's');
        }

        static void OnActivate(object sender, ActivationEventArgs e)
        {
            Neuron temp = (Neuron)sender;
            Console.WriteLine("The output for the neuron {2} is {0} when the input is {1}", e.Activation, e.Input, temp.ID);
            if(temp.ID == outputID)
                activationFlag = false;
        }
    }
}
