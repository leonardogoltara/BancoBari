using HelloWorldDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorldBus
{
    public interface IBusService
    {
        void Initialize(string hostName, string keyQueue);
        void Send(Message message);
        void Receive();
    }
}
