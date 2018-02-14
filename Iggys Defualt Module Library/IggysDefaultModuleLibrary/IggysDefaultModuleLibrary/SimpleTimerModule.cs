using System;
using ThreadHelper_Library;
using IggysQSM;
using System.Timers;
using System.Collections.Generic;
using System.Linq;

namespace IggysDefaultModuleLibrary
{
    public class SimpleTimerModule : TSubject<CommQueueDefaultData>
    {
        // This class is used as an outline for every other module in the library.

        private QSM QSM;            // State machine service for the module
        private List<TimerData> timers; // List of timers that this module controls
        private int timerCount;

        // Constructor
        public SimpleTimerModule():base()
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
                        timerCount = 0;
                        timers = new List<TimerData>();
                        break;

                    case "Event Check":
                        // If there's any events that have been added to the mailbox, then attend to them
                        while(mailbox.Count > 0)
                        {
                            CommQueueDefaultData mail = mailbox.Dequeue();
                            switch(mail.Command)
                            {
                                case "Add Timer":
                                    // Adds a timer to the list
                                    AddTimer((TimerData)mail.Data);
                                    timerCount++;
                                    break;

                                case "Remove Timer":
                                    // Removes a timer from the list
                                    RemoveTimer((int)mail.Data);
                                    timerCount--;
                                    break;

                                case "Exit":
                                    // Causes the module to exit
                                    Exit();
                                    break;
                            }
                        }
                        break;

                    case "MsgTimerPing":
                        // Sends a message to the network stating that a timer has been fired
                        OnMessageTx(new MessageEventArgs<CommQueueDefaultData>(
                            new CommQueueData<CommQueueDefaultData>(
                                new CommQueueDefaultData("Timer Ping!", CurrentState.Data))));
                        break;

                    default:
                        // Occurs when there's a state in the Queue that doesn't match any of the cases
                        Console.WriteLine("Unrecognized state: {0}", CurrentState.State);
                        break;
                }
            }
        }

        private void AddTimer(TimerData data)
        {
            // Adds a new timer to the list of timers
            timers.Add(data);
            // Signs up for it's timer event
            timers.Last().ElapsedEvent += OnElapsed;
        }

        private void RemoveTimer(int id)
        {
            // Removes a timer from the list of timers
            for (int i = 0; i < timers.Count; i++)
                if (timers[i].Id == id)
                    timers.RemoveAt(i);
        }

        protected virtual void OnElapsed(object source)
        {
            // Triggered whenever a timer fires, it checks if the timer is meant to repeat and removes it if it's not.
            // Then queues a state to notify the listener.
            TimerData temp = (TimerData)source;
            if(!temp.Repeat)
            {
                RemoveTimer(temp.Id);
            }
            QSM.AddStates(new QSMStateData("MsgTimerPing", temp.Id));
        }

        public class TimerData
        {
            public delegate void OnElapsedEventHandler(object source);
            public event OnElapsedEventHandler ElapsedEvent;

            // Used for keeping track of timer data
            private int id;
            private Timer timer;
            private bool repeat;

            public int Id { get => id; set => id = value; }
            public Timer Timer { get => timer; set => timer = value; }
            public bool Repeat { get => repeat; set => repeat = value; }

            public TimerData(Timer timer, int id = 0, bool repeat = false)
            {
                this.id = id;
                this.timer = timer;
                this.repeat = repeat;
            }

            protected virtual void OnElapsed(object source, ElapsedEventArgs e)
            {
                // Triggered then the timer trips
                ElapsedEvent?.Invoke(this);
            }
        }

        //Override method for the messageRx event
        protected override void OnMessageRx(CommQueueDefaultData e)
        {
            // Adds any received messages to the mailbox
            base.OnMessageRx(e);
            //QSMStateData temp = new QSMStateData(e.Command, e.Data);
            //QSM.AddStates(temp);
        }
    }
}
