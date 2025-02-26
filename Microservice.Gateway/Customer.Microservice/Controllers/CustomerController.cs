using Microsoft.AspNetCore.Mvc;

namespace Customer.Microservice.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
