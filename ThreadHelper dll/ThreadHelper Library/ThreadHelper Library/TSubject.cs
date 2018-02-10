using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadHelper_Library
{
    public abstract class TSubject<T>
    {
        /* This class is to be a parent to any class that you want to be executed on the separate thread.
         * It provides a method for the ThreadCaller class to execute when the process should be launched.
         */

        private long id;
        private static long modCount;
        private Queue<T> mailbox;
        private bool isExitting;

        private string name;    // Used for addressing this module in messages.

        public string Name { get => name; set => name = value; }
        public long Id { get => id; set => id = value; }
        public static long ModCount { get => modCount; set => modCount = value; }

        //Constructor
        public TSubject()
        {
            id = ++modCount;   // sets the unique id number and increments the modCount counter
            isExitting = false;

            mailbox = new Queue<T>();
            Thread mailboxThread = new Thread(new ThreadStart(MailboxManager));
            mailboxThread.Start();
        }

        public virtual void Exit()
        {
            isExitting = true;
        }

        // Must be called on the start of the thread call
        public abstract void Start();

        // Events for sending and receiving messages from other threads and modules

        // Delegates
        public delegate void MessageRxEventHandler(object Sender, T e);
        public delegate void MessageTxEventHandler(object Sender, MessageEventArgs<T> e);

        // Events
        public event MessageRxEventHandler MessageRx;
        public event MessageTxEventHandler MessageTx;

        // Event Methods
        protected virtual void OnMessageRx(T e)
        {
            MessageRx?.Invoke(this, e);
        }

        protected virtual void OnMessageTx(MessageEventArgs<T> e)
        {
            MessageTx?.Invoke(this, e);
        }

        private void MailboxManager()
        {
            // Monitors the mailbox for messages, if it receives one, it triggers the MessageRx event.
            while(!isExitting)
            {
                if(mailbox.Count != 0)
                {
                    OnMessageRx(mailbox.Dequeue());
                }
            }
        }

        public void ReceiveMailbox(T message)
        {
            // Receives a message
            // Is a method for the ThreadLauncher to place mail into the mailbox
            mailbox.Enqueue(message);   // Receives the message
        }
    }

    public class MessageEventArgs<T> : EventArgs
    {
        // This Class embodies the arguments sent and received in messages.
        CommQueueData<T> message;

        public CommQueueData<T> Message { get => message; set => message = value; }

        public MessageEventArgs(CommQueueData<T> message)
        {
            this.message = message;
        }
    }
}
