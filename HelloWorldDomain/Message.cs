using System;

namespace HelloWorldDomain
{
    public sealed class Message
    {
        public Message() : this("", Guid.Empty)
        { }

        public Message(string textMessage, Guid serviceID)
        {
            ID = Guid.NewGuid();
            ServiceID = serviceID;
            Date = DateTime.Now;
            TextMessage = textMessage;
        }

        public Guid ID { get; set; }
        public Guid ServiceID { get; set; }
        public string TextMessage { get; set; }
        public DateTime Date { get; set; }

        public void ChangeServiceID(Guid serviceID)
        {
            if (serviceID == Guid.Empty)
                throw new Exception("ServiceID inválido.");

            ServiceID = serviceID;
        }
    }
}
