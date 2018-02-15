using IggysModularProjectTemplate.Modules;
using System;
using System.Collections.Generic;
using ThreadHelper_Library;

namespace IggysModularProjectTemplate
{
    class Program
    {
        static ThreadLauncher<CommQueueDefaultData> threadManager;

        static void Main(string[] args)
        {
            // This program is responsible for managing and launching all of the modules found in Modules
            Console.WriteLine("Setting up...");
            threadManager = new ThreadLauncher<CommQueueDefaultData>();
            Console.WriteLine("- Generating modules");
            List<TSubject<CommQueueDefaultData>> modules = new List<TSubject<CommQueueDefaultData>>();      // This list contains all of the modules you need to launch

            // Add all of your modules here:
            modules.Add(new SampleModule());

            Console.WriteLine("  * Adding {0} Modules to Thread Manager", modules.Count);
            foreach (TSubject<CommQueueDefaultData> module in modules)
                threadManager += module;

            Console.WriteLine("- Launching Modules");
            threadManager.LaunchAll();

            Console.WriteLine("Launch Complete!");
            while (threadManager.IsActive) ;        // Waits until all the modules are done executing
            threadManager.Exit();                   // Shuts down the threadLauncher
            Console.ReadKey();
        }
    }
}
