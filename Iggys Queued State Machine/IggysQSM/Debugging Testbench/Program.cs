using IggysQSM;
using System;

namespace Debugging_Testbench
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            QSM qsm = new QSM();
            bool isExitting = false;
            while(!isExitting)
            {
                Console.WriteLine("The next state is: {0}", qsm.GetNextState().State);
            }
        }
    }
}
