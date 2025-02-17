using Microsoft.AspNetCore.Mvc;
using E_CommerceBackend.DTOs;
using E_CommerceBackend.Services;
    
namespace E_CommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProductController : ControllerBase
    {
        private readonly AdminProductService _adminProductService;

        public AdminProductController(AdminProductService adminProductService)
        {
            _adminProductService = adminProductService;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _adminProductService.GetAllProductsAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet]
        [Route("GetProductsByUserAsync")]
        public async Task<IActionResult> GetProductsByUserAsync(int UsertableId)
        {
            var result = await _adminProductService.GetProductsByUserAsync(UsertableId);
            return StatusCode(result.Status, result);
        }



        [HttpGet]
        [Route("GetProductById/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var result = await _adminProductService.GetProductByIdAsync(productId);
            return StatusCode(result.Status, result);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(Productdto productDto)
        {
            var result = await _adminProductService.AddProductAsync(productDto);
            return StatusCode(result.Status, result);
        }

        [HttpPut]
        [Route("UpdateProduct/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, Productdto productDto)
        {
            var result = await _adminProductService.UpdateProductAsync(productId, productDto);
            return StatusCode(result.Status, result);
        }

        [HttpDelete]
        [Route("DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _adminProductService.DeleteProductAsync(productId);
            return StatusCode(result.Status, result);
        }
    }

}
