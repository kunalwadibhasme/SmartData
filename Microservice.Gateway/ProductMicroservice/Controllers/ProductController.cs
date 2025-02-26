using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.Dtos;
using ProductMicroservice.Service;

namespace ProductMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        public readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddproductDto addproductDto)
        {
            var result = await _productService.AddProduct(addproductDto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int Id)
        {
            var result = await _productService.GetProductById(Id);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int Id, AddproductDto addproductDto)
        {
            var result = await _productService.UpdateProduct(Id, addproductDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var result = await _productService.DeleteProduct(Id);
            return Ok(result);
        }
    }
}
