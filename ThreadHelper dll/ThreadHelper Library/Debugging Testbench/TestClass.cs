using System;
using System.Collections.Generic;
using System.Text;
using ThreadHelper_Library;

namespace Debugging_Testbench
{
    class TestClass : TSubject<string>
    {
        public override void Start()
        {
            Console.WriteLine("Hello World from thread {0}", Id);
            OnMessageTx(new MessageEventArgs<string>(new CommQueueData<string>("Hello World from thread " + Id)));
            //Exit();
        }

        protected override void OnMessageTx(MessageEventArgs<string> e)
        {
            base.OnMessageTx(e);
        }
    }
}
