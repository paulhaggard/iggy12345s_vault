using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadHelper_Library
{
    class ThreadLauncher<T>
    {
        // Contains and launches threads as told to.
        List<Thread> Threads;   // List of threads to be launched.
        bool isLaunched;        // Used to tell when the threads have been launched
        Queue<T> CommQueue;     // Queue used for callback communication

    }
}
