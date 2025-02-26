using E_CommerceBackend.DTOs;
using E_CommerceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
                _cartService = cartService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddMasterDetails(CartMasterDetaildto cartMasterDetaildto)
        {
            var result = await _cartService.AddinMasterDetail(cartMasterDetaildto);
            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ReduceStockofproduct(int Productid)
        {
            var result = await _cartService.ReduceStockofProduct(Productid);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> FetchUserAddedProduct(int Id)
        {
            var result = await _cartService.FetchUserAddedProduct(Id);
            return Ok(result);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> ReducefromMastertables( int productId, int userTableId)
        {
            var result = await _cartService.ReducefromMastertables(productId, userTableId);
            return Ok(result);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Updatesalesdetails(int UserId, float total)
        {
            var result = await _cartService.Updatesalesdetails(UserId, total);
            return Ok(result);
        }


        [HttpDelete("[action]")]
        public async Task<IActionResult> Updatesalesdetails2(int UserId, float total)
        {
            var result = await _cartService.Updatesalesdetails2(UserId, total);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidateDetails(Carddto card)
        {
            var result = await _cartService.ValidateDetail(card);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetSalesDetail(int UserId)
        {
            var result = await _cartService.GetSalesDetails(UserId);
            return Ok(result);
        }
       
    }
}
