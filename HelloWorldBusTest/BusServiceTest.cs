using HelloWorldBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloWorldBusTest
{
    [TestClass]
    public class BusServiceTest
    {
        [TestMethod]
        public void BusService_ConstructorEmpty_OK()
        {
            BusService bus = new BusService();

            Assert.IsTrue(string.IsNullOrEmpty(bus.HostName));
            Assert.IsTrue(string.IsNullOrEmpty(bus.KeyQueue));
        }

        [TestMethod]
        public void BusService_ConstructorFilled_OK()
        {
            BusService bus = new BusService("hostname", "key");

            Assert.IsFalse(string.IsNullOrEmpty(bus.HostName));
            Assert.IsFalse(string.IsNullOrEmpty(bus.KeyQueue));
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
        public void BusService_Receive_NOK()
        {
            BusService bus = new BusService();

            bus.Receive();
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
        public void BusService_Send_NOK()
        {
            BusService bus = new BusService();

            bus.Send(new HelloWorldDomain.Message());
        }
    }
}
