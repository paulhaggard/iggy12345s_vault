using System;
using ThreadHelper_Library;
using IggysQSM;

namespace Project_Template
{
    public class Test_QSM_Module : TSubject<CommQueueDefaultData>
    {
        // This class is used as an outline for every other module in the library.

        private bool isExitting;    // Determines when the module is closing down
        private QSM QSM;            // State machine service for the module

        // Constructor
        public Test_QSM_Module():base()
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
                        Console.WriteLine("Hello World!");
                        break;

                    case "Event Check":
                        // If there's any events that have been added to the mailbox, then attend to them
                        break;

                    case "Module Exit":
                        isExitting = true;
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
