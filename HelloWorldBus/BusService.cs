using HelloWorldDomain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace HelloWorldBus
{
    public sealed class BusService
    {
        public BusService(string hostName, string keyQueue)
        {
            HostName = hostName;
            KeyQueue = keyQueue;
        }
        public string HostName { get; }
        public string KeyQueue { get; }
        public void Send(Message message)
        {
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
    }
}
