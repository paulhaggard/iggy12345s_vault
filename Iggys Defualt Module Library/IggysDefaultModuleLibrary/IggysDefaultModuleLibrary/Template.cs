using System;
using ThreadHelper_Library;

namespace IggysDefaultModuleLibrary
{
    public class Template : TSubject<CommQueueDefaultData>
    {
        // This class is used as an outline for every other module in the library.

        private bool isExitting;

        // Constructor
        public Template():base()
        {
            isExitting = false;
        }

        // Implements the required start method for the TSubject class
        public override void Start()
        {

        }
    }
}
