using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace NotificationService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailService _emailService;

        public Worker(ILogger<Worker> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;


        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = "microservice-rabbitmq-1",
                UserName = "guest",
                Password = "guest",
                
            };

            
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "orderQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            //stoppingToken.Register(() => _logger.LogInformation("Stopping the service."));
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var order = JsonSerializer.Deserialize<Order>(message);
                _logger.LogInformation("Received message: {Message}", message);
               // _emailService.SendEmailAsync(message, "New Order Notification", $"Order for with quantity has been received.");
                _emailService.SendEmailAsync(order.Email, "New Order Notification","Order for Product " + order.ProductName + " of Quantity " + order.Quantity+" Has Been Placed");

                try
                {
                    _logger.LogInformation("Email sent to {Email}", message);
                   // _emailService.SendEmailAsync(message, "New Order Notification", $"Order for with quantity has been received.");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing the message.");
                }
            };

            channel.BasicConsume(queue: "orderQueue", autoAck: true, consumer: consumer);
            _logger.LogInformation("RabbitMQ consumer started.");

            return Task.CompletedTask;
        }

        //public override void Dispose()
        //{
        //    var factory = new ConnectionFactory
        //    {
        //        HostName = "microservice-rabbitmq-1",
        //        UserName = "guest",
        //        Password = "guest"
        //    };
        //    using var connection = factory.CreateConnection();
        //    using var channel = connection.CreateModel();

        //    channel.Close();
        //    connection.Close();
        //    _logger.LogInformation("RabbitMQ connection and channel closed.");
        //    base.Dispose();
        //}
    }
    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Email { get; set; }

    }
}
