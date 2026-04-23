using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetailOrdering.API.DTOs.Order;
using RetailOrdering.API.Helpers;
using RetailOrdering.API.Services;
using System.Security.Claims;

namespace RetailOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cart;
        public CartController(CartService cart) => _cart = cart;
        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet] public async Task<IActionResult> GetCart() => Ok(ApiResponse<CartDto>.Ok(await _cart.GetCartAsync(UserId)));
        [HttpPost("add")] public async Task<IActionResult> Add(CartItemDto dto) { await _cart.AddToCartAsync(UserId, dto); return Ok(ApiResponse<string>.Ok("Added to cart")); }
        [HttpDelete("{productId}")] public async Task<IActionResult> Remove(int productId) { await _cart.RemoveFromCartAsync(UserId, productId); return Ok(ApiResponse<string>.Ok("Removed")); }
        [HttpDelete("clear")] public async Task<IActionResult> Clear() { await _cart.ClearCartAsync(UserId); return Ok(ApiResponse<string>.Ok("Cart cleared")); }
    }
}
