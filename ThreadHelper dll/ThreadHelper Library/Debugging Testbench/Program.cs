using System;
using System.Collections.Generic;
using ThreadHelper_Library;

namespace Debugging_Testbench
{
    class Program
    {
        static int completed = 0;
        static ThreadLauncher<string> threadLauncher;
        static List<TestClass> modules;
        // To do:
        // make the threadLauncher exit after receiving all of the messages from all of the threads.
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            modules = new List<TestClass>(20);
            threadLauncher = new ThreadLauncher<string>();
            threadLauncher.MessageRx += OnMessageRx;
            for (int i = 0; i < 20; i++)
            {
                // Creates the modules
                Console.WriteLine("Creating Module {0}", i);
                modules.Add(new TestClass());               
            }
            // Adds the modules to the threadlauncher
            foreach (TestClass module in modules)
            {
                Console.WriteLine("Adding Module {0}", module.Id);
                threadLauncher.Add(module);
            }

            Console.WriteLine("Launching modules...");
            threadLauncher.LaunchAll();

            Console.WriteLine("Testing CommQueueData, and CommQueueDefualtData...");
            CommQueueData<CommQueueDefaultData> data = new CommQueueData<CommQueueDefaultData>(new CommQueueDefaultData("Hello"));
        }

        static void OnMessageRx(MessageEventArgs<string> e)
        {
            Console.WriteLine("Received a message!");
            completed++;
            if (completed >= 19)
                threadLauncher.Exit();
        }
    }
}
