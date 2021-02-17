using HelloWorldDomain;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HelloWorldApp
{
    class Program
    {
        public static Guid ServiceID;
        static void Main(string[] args)
        {
            ServiceID = Guid.NewGuid();
            Console.WriteLine("ServiceID: " + ServiceID);

            var client = new HttpClient();
            var uri = "http://localhost:5000/api/message";

            while (true)
            {
                var message = new Message("Hello World!", ServiceID);
                var jsonInString = JsonSerializer.Serialize(message);
                client.PostAsync(uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));

                Console.WriteLine($"## Sent ## ServiceID: {message.ServiceID} | ID:{message.ID} | Message: {message.TextMessage} | Date: {message.Date}");

                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}
