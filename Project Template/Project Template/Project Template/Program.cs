using System;
using System.Collections.Generic;
using ThreadHelper_Library;

namespace Project_Template
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting System!");
            ThreadLauncher<CommQueueDefaultData> ThreadHandler = new ThreadLauncher<CommQueueDefaultData>();    // Initializes the thread handler
            List<TSubject<CommQueueDefaultData>> Modules = new List<TSubject<CommQueueDefaultData>>();          // Creates an instance of the module list
            /* 
             * Add your custom modules to launch asynchronously here.
             */

                                                                                                                // Adds all of the modules to the Handler
            foreach (TSubject<CommQueueDefaultData> item in Modules)
                ThreadHandler.Add(item);

            Console.WriteLine("Preparing to launch modules...");
            

        }
    }
}
