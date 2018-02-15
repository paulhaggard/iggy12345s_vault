using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadHelper_Library
{
    public class ThreadLauncher<T>
    {
        // Contains and launches threads as told to.

        public delegate void MessageRxHandler(MessageEventArgs<T> e);
        public event MessageRxHandler MessageRx;

        private List<TSubject<T>> modules;              // List of the modules in the list of threads
        private List<Action> Tasks;                     // List of threads to be launched.
        private List<bool> isLaunched;                  // Used to tell when the threads have been launched
        private Queue<CommQueueData<T>> CommQueue;      // Queue used for callback communication
        private bool isExitting;

        public bool IsActive { get => checkActivity(); }

        private bool checkActivity()
        {
            // Determines if any of the modules are currently running
            bool temp = false;
            foreach (bool activity in isLaunched)
                temp = activity ? true : temp;
            return temp;
        }

        public ThreadLauncher()
        {
            Tasks = new List<Action>();
            modules = new List<TSubject<T>>();
            isLaunched = new List<bool>();
            CommQueue = new Queue<CommQueueData<T>>();
            isExitting = false;

            Task TxThread = new Task(new Action(TxManager));
            TxThread.Start();   // Starts the TxManager
        }

        public void Exit()
        {
            // Sends a mass exit message to all modules notifying to shut down.
            foreach (TSubject<T> module in modules)
                module.Exit();

            // Waits for any last minute shutdown procedures
            bool isDone = false;
            int iter = 0;
            do
            {
                isDone = true;
                foreach (Action t in Tasks)
                    if (t == null ? true:false)
                        isDone = false;

            } while (!isDone && iter++ < 5000);
        }

        public void Add(TSubject<T> module)
        {
            // Adds a module to the list of handled threads
            modules.Add(module);                                    // Adds the module to the list
            module.MessageTx += OnMessageRx;                        // Subscribes to the module's messageTx event
            Tasks.Add(new Action(module.Start));                    // Adds the thread to the list
            isLaunched.Add(false);                                  // Adds a boolean status flag to the list as well
        }

        public void Add(List<TSubject<T>> modules)
        {
            // Adds a module to the list of handled threads
            foreach (TSubject<T> module in modules)
                Add(module);
        }

        public void Add(TSubject<T>[] modules)
        {
            Add(modules.ToList());
        }

        public void Remove(int index, bool overrideLaunch = false)
        {
            // Removes a module from the list of handled threads
            if (isLaunched[index] && !overrideLaunch)
                throw new InvalidOperationException("Cannot halt a thread that is already launched!");
            else
            {
                // Removes the thread from the list and its corresponding status boolean

                if(isLaunched[index])
                    Abort(index);       // Aborts the task if it's running.

                Tasks.RemoveAt(index);
                isLaunched.RemoveAt(index);
                modules.RemoveAt(index);
            }
        }

        public void Remove(TSubject<T> module, bool overrideLaunch = false)
        {
            // Removes a module from the list of handled threads
            if (modules.Contains(module))
                Remove(modules.FindIndex(x => x.Id == module.Id), overrideLaunch);
            else
                throw new InvalidOperationException("The thread list does not contain that module!");
        }

        // Methods for launching processes
        public async void LaunchAll()
        {
            // Launches all of the modules
            for(int i = 0; i < Tasks.Count; i++)
            {
                await Task.Run(Tasks[i]);       // Calls the method asynchronously
            }
        }

        public async void Launch(int index)
        {
            // Launches an individual module
            if (isLaunched[index])
                throw new InvalidOperationException("Can't launch a thread that's already launched!");
            isLaunched[index] = true;
            await Task.Run(Tasks[index]);       // Calls the method asynchronously
        }

        public void Launch(List<int> index)
        {
            // Launches a select list of modules
            foreach (int i in index)
            {
                Launch(i);
            }
        }

        public void Launch(int[] index)
        {
            // Launches a select list of modules
            Launch(index.ToList());
        }

        // Methods for Aborting processes
        public void AbortAll()
        {
            // Launches all of the modules
            for (int i = 0; i < Tasks.Count; i++)
            {
                Abort(i);
            }
        }

        public void Abort(int index)
        {
            // Stops an individual module
            if (!isLaunched[index])
                throw new InvalidOperationException("Can't abort a thread that isn't already launched!");

            modules[index].Exit();                  // Tells the module to stop

            while (!modules[index].IsExitting) ;    // Waits until the module has finished exitting

            isLaunched[index] = false;              // Sets the status boolean
        }

        public void Abort(List<int> index)
        {
            // Stops a select list of modules
            foreach (int i in index)
            {
                Abort(i);
            }
        }

        public void Abort(int[] index)
        {
            // Stops a select list of modules
            Abort(index.ToList());
        }

        // Operators
        public static ThreadLauncher<T> operator +(ThreadLauncher<T> launcher, TSubject<T> module)
        {
            // Adds the modules to the lists
            launcher.Add(module);
            return launcher;
        }

        public static ThreadLauncher<T> operator -(ThreadLauncher<T> launcher, TSubject<T> module)
        {
            // Adds the modules to the lists
            launcher.Remove(module);
            return launcher;
        }

        protected virtual void OnMessageRx(object sender, MessageEventArgs<T> e)
        {
            // Triggered when another module tries to send out a message
            CommQueue.Enqueue(e.Message);   // Enqueues the message
            MessageRx?.Invoke(e);
        }

        protected virtual void OnMessageTx(CommQueueData<T> e)
        {
            // Sends a message from the queue as soon as it's queued
            if (e.Type == CommMessageType.Addressed)
            {
                // Sends an addressed message to all modules that have the name in the message
                if (modules.Find(x => e.Addressee == x.Name) != null)
                    foreach (TSubject<T> item in modules.FindAll(x => e.Addressee == x.Name))
                        item.ReceiveMailbox(e.Message); // Inserts the message into the mailbox
            }
            else if (e.Type == CommMessageType.UnAddressed)
            {
                // Sends a message to every module
                foreach (TSubject<T> module in modules)
                    module.ReceiveMailbox(e.Message);
            }
            else
                throw new IndexOutOfRangeException("Message Type unknown!");
        }

        private void TxManager()
        {
            // Monitors the CommQueue, sending messages as needed
            while(!isExitting)
            {
                if (CommQueue.Count > 0)
                    OnMessageTx(CommQueue.Dequeue());
            }
        }
    }
}
