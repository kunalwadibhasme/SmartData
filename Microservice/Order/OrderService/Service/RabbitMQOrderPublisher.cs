using Microsoft.EntityFrameworkCore.Metadata;
using OrderService.Model;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace OrderService.Service
{
    public class RabbitMQOrderPublisher
    {
        public void PublishOrder(Order order)
        {
            var factory = new ConnectionFactory
            {
                HostName = "microservice-rabbitmq-1",
                UserName = "guest",
                Password = "guest",
                
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "orderQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var orderJson = JsonSerializer.Serialize(order);
            var body = Encoding.UTF8.GetBytes(orderJson);

            channel.BasicPublish(exchange: "", routingKey: "orderQueue", basicProperties: null, body: body);
            Console.WriteLine($"[OrderService] Order published: {order.Id}");
        }
    }
}
