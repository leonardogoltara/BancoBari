using HelloWorldDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HelloWorldDomainTest
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void Message_ConstructorEmpty_OK()
        {
            Message message = new Message();

            Assert.AreEqual(Guid.Empty, message.ServiceID);
            Assert.AreNotEqual(Guid.Empty, message.ID);
            Assert.IsTrue(string.IsNullOrEmpty(message.TextMessage));
            Assert.AreNotEqual(new DateTime(), message.Date);
        }

        [TestMethod]
        public void Message_ConstructorFilled_OK()
        {
            var guid = Guid.NewGuid();
            var strmessage = "Mensagem";

            Message message = new Message(strmessage, guid);

            Assert.AreEqual(guid, message.ServiceID);
            Assert.AreNotEqual(Guid.Empty, message.ID);
            Assert.AreEqual(strmessage, message.TextMessage);
            Assert.AreNotEqual(new DateTime(), message.Date);
        }

        [TestMethod]
        public void Message_ChangeServiceID_OK()
        {
            var serviceID = Guid.NewGuid();
            var strmessage = "Mensagem";

            Message message = new Message(strmessage, serviceID);

            var newServiceID = Guid.NewGuid();

            message.ChangeServiceID(newServiceID);

            Assert.AreEqual(newServiceID, message.ServiceID);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
        public void Message_ChangeServiceID_NOK()
        {
            var serviceID = Guid.NewGuid();
            var strmessage = "Mensagem";

            Message message = new Message(strmessage, serviceID);

            var newServiceID = Guid.Empty;

            message.ChangeServiceID(newServiceID);

            Assert.AreEqual(newServiceID, message.ServiceID);
        }
    }
}
