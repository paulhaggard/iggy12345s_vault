using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadHelper_Library
{
    public enum CommMessageType { UnAddressed = 0, Addressed };

    public class CommQueueData<T>
    {
        // This is used to send and receive data to and from the asynchronous modules.
        private CommMessageType type;
        private T message;
        private string addressee;

        public CommMessageType Type { get => type; set => type = value; }
        public T Message { get => message; set => message = value; }
        public string Addressee { get => addressee; set => addressee = value; }

        public CommQueueData(T message, CommMessageType type = CommMessageType.UnAddressed, string addressee = "")
        {
            this.message = message;
            this.type = type;
            this.addressee = addressee;
        }
    }
}
