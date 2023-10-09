using Microsoft.AspNetCore.Connections;
using System.Text;
using Micro.Domain.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Micro.ToDo.Service
{
    public class EmailProducer
    {
        public void SendEmailRequest(EmailRequest emailRequest)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "email_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = JsonConvert.SerializeObject(emailRequest);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "email_queue", basicProperties: null, body: body);
                Console.WriteLine("Email request sent: {0}", message);
            }
        }
    }
}
