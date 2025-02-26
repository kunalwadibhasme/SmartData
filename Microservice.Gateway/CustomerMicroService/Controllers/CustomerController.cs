using CustomerMicroService.Dtos;
using CustomerMicroService.Service;
using Microsoft.AspNetCore.Mvc;

namespace CustomerMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {

        public readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomer addCustomer)
        {
            var result = await _customerService.AddCustomer(addCustomer);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int Id, AddCustomer addCustomer)
        {
            var result = await _customerService.UpdateCustomer(Id, addCustomer);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            var result = await _customerService.GetAllCustomer();
            return Ok(result);
        }
    }
}
