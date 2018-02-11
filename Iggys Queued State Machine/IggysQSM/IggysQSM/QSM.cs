using System;
using System.Collections.Generic;

namespace IggysQSM
{
    public struct QSMStateData
    {
        // Used as the data structure for the qsm
        private string state;
        private object data;

        public QSMStateData(string state, object data = null)
        {
            this.state = state;
            this.data = data;
        }

        public string State { get => state; set => state = value; }
        public object Data { get => data; set => data = value; }
    }

    public class QSM
    {
        // Allows to easy creation of a queued state machine

        private Queue<QSMStateData> states; // Queue for the state machine
        private string initialState;        // Initial state for the machine, this always executes firs
        private string defaultState;        // The default state to goto if the queued string is ""

        public string InitialState { get => initialState; set => initialState = value; }
        public string DefaultState { get => defaultState; set => defaultState = value; }

        // Constructor
        public QSM(string initialState = "Module Initialize", string exitState = "Module Exit", string defaultState = "Idle")
        {
            states = new Queue<QSMStateData>();
            this.initialState = initialState;
            this.defaultState = defaultState;

            // Queues the first state to execute
            AddStates(initialState);
        }

        // Methods for queueing states
        public void AddStates(QSMStateData state)
        {
            // Adds a state to the queue
            states.Enqueue(state);
        }

        public void AddStates(List<QSMStateData> states)
        {
            // Adds multiple states to the queue
            foreach (QSMStateData state in states)
                AddStates(state);
        }

        public void AddStates(string state = "")
        {
            // Adds a state to the queue from string data
            AddStates(new QSMStateData(state));
        }

        public void AddStates(string[] states)
        {
            // Adds multiple states to the queue from a string array
            foreach (string state in states)
                AddStates(state);
        }

        public void AddStates(List<string> states)
        {
            // Adds multiple states to the queue from a string List
            foreach (string state in states)
                AddStates(state);
        }

        // Operators for queueing states
        public static QSM operator +(QSM machine, QSMStateData state)
        {
            machine.AddStates(state);
            return machine;
        }

        public static QSM operator +(QSM machine, List<QSMStateData> states)
        {
            machine.AddStates(states);
            return machine;
        }

        public static QSM operator +(QSM machine, string state)
        {
            machine.AddStates(state);
            return machine;
        }

        public static QSM operator +(QSM machine, string[] state)
        {
            machine.AddStates(state);
            return machine;
        }

        public static QSM operator +(QSM machine, List<string> states)
        {
            machine.AddStates(states);
            return machine;
        }

        public static QSM operator ++(QSM machine)
        {
            // Adds the default state
            machine.AddStates();
            return machine;
        }

        // Methods for dequeueing states
        public QSMStateData GetNextState()
        {
            // Returns the next state to be executed
            if(states.Count > 0)
                return (states.Peek().State == "") ? new QSMStateData(defaultState) : states.Dequeue();
            return new QSMStateData(defaultState);
        }

        public List<QSMStateData> GetNextStates(int num = 1)
        {
            // Returns the next states to be executed
            List<QSMStateData> temp = new List<QSMStateData>();

            if (states.Count >= num)
            {
                temp = new List<QSMStateData>(num);
                for (int i = 0; i < num; i++)
                    temp.Add(states.Dequeue());
                return temp;
            }
            else
            {
                temp.Add(new QSMStateData(DefaultState));
                return temp;
            }
        }

        // Operators for dequeueing states
        public static List<QSMStateData> operator -(QSM machine, int num)
        {
            return machine.GetNextStates(num);
        }

        public static QSM operator --(QSM machine)
        {
            // Throws away a state, does not actually give you any information about it
            machine.GetNextState();
            return machine;
        }

        // Methods for clearing the queue
        public List<QSMStateData> Flush()
        {
            // Flushes all of the states from the buffer and returns the remaining ones
            List<QSMStateData> temp = new List<QSMStateData>(states.Count);

            while (states.Count > 0)
                temp.Add(states.Dequeue());

            return temp;
        }

    }
}
