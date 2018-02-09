using System;

namespace ThreadHelper_Library
{
    public interface TSubject
    {
        /* This interface is to be a parent to any class that you want to be executed on the separate thread.
         * It provides a method for the ThreadCaller class to execute when the process should be launched.
         */
        void Start();
    }
}
