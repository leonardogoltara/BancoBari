using HelloWorldDomain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace HelloWorldBus
{
    public sealed class BusService : IBusService
    {
        public BusService() : this("", "")
        {

        }
        public BusService(string hostName, string keyQueue)
        {
            Initialize(hostName, keyQueue);
        }
        public string HostName { get; private set; }
        public string KeyQueue { get; private set; }
        public void Send(Message message)
        {
            Validate();

            var factory = new ConnectionFactory() { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: KeyQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string strMessage = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(strMessage);

                channel.BasicPublish(exchange: "",
                                     routingKey: KeyQueue,
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"## Sent ## ServiceID: {message.ServiceID} | ID:{message.ID} | Message: {message.TextMessage} | Date: {message.Date}", strMessage);
            }

        }
        public void Receive()
        {
            Validate();

            var factory = new ConnectionFactory() { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: KeyQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var strMessage = Encoding.UTF8.GetString(body);

                        Message message = JsonSerializer.Deserialize<Message>(strMessage);

                        Console.WriteLine($"## Received ## ServiceID: {message.ServiceID} | ID:{message.ID} | Message: {message.TextMessage} | Date: {message.Date}", strMessage);

                        System.Threading.Thread.Sleep(1000);

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception)
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                        throw;
                    }
                };
                channel.BasicConsume(queue: KeyQueue,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
        public void Initialize(string hostName, string keyQueue)
        {
            HostName = hostName;
            KeyQueue = keyQueue;
        }
        private void Validate()
        {
            if (string.IsNullOrEmpty(HostName) || string.IsNullOrEmpty(KeyQueue))
                throw new Exception("Serviço não iniciado.");
        }
    }
}
