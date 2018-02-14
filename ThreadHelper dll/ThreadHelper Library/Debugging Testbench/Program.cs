using System;
using ThreadHelper_Library;

namespace Debugging_Testbench
{
    class Program
    {
        static int completed = 0;
        // To do:
        // make the threadLauncher exit after receiving all of the messages from all of the threads.
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            ThreadLauncher<string> threadLauncher = new ThreadLauncher<string>();
            threadLauncher.MessageRx += OnMessageRx;
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("Creating Thread {0}", i);
                threadLauncher.Add(new TestClass());
            }
            threadLauncher.LaunchAll();
        }

        static void OnMessageRx(MessageEventArgs<string> e)
        {
            Console.WriteLine("Received a message!");
        }
    }
}
