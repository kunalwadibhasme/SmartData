using Microsoft.AspNetCore.Mvc;
using OrderService.Model;
using OrderService.Service;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly RabbitMQOrderPublisher _publisher;
        public OrderController(RabbitMQOrderPublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost]
        public IActionResult PLaceAnOrder([FromBody] Order order)
        {
            _publisher.PublishOrder(order);
            return Ok(new { message = "Order Has Been Placed Successfully" });
        }
    }
}
