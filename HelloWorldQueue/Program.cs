using System;

namespace HelloWorldQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            var srv = new HelloWorldBus.BusService("localhost", "HelloWorldAPI");
            srv.Receive();
        }
    }
}
