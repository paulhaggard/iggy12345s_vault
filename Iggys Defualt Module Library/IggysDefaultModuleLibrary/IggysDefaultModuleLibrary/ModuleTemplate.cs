using System;
using ThreadHelper_Library;
using IggysQSM;

namespace IggysDefaultModuleLibrary
{
    public class ModuleTemplate : TSubject<CommQueueDefaultData>
    {
        // This class is used as an outline for every other module in the library.

        private QSM QSM;            // State machine service for the module

        // Constructor
        public ModuleTemplate():base()
        {
            isExitting = false;

            // Sets up the QSM for this module
            QSM = new QSM(defaultState: "Event Check");
        }

        // Implements the required start method for the TSubject class
        public override void Start()
        {
            while(!isExitting)
            {
                // Dequeus the next state
                QSMStateData CurrentState = QSM.GetNextState();
                switch(CurrentState.State)
                {
                    case "Module Initialize":
                        // Assign any initial values to things here
                        break;

                    case "Event Check":
                        // If there's any events that have been added to the mailbox, then attend to them
                        break;

                    default:
                        // Occurs when there's a state in the Queue that doesn't match any of the cases
                        Console.WriteLine("Unrecognized state: {0}", CurrentState.State);
                        break;
                }
            }
        }

        //Override method for the messageRx event
        protected override void OnMessageRx(CommQueueDefaultData e)
        {
            // Adds any received messages as states to the QSM
            base.OnMessageRx(e);
            QSMStateData temp = new QSMStateData(e.Command, e.Data);
            QSM.AddStates(temp);
        }
    }
}
