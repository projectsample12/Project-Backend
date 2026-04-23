using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetailOrdering.API.DTOs.Order;
using RetailOrdering.API.Helpers;
using RetailOrdering.API.Models;
using RetailOrdering.API.Services;
using System.Security.Claims;

namespace RetailOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orders;
        public OrderController(OrderService orders) => _orders = orders;
        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("place")] public async Task<IActionResult> Place(OrderRequestDto dto) => Ok(ApiResponse<OrderConfirmationDto>.Ok(await _orders.PlaceOrderAsync(UserId, dto)));
        [HttpGet("my")] public async Task<IActionResult> MyOrders() => Ok(ApiResponse<List<OrderDto>>.Ok(await _orders.GetUserOrdersAsync(UserId)));
        [HttpGet, Authorize(Roles = "Admin")] public async Task<IActionResult> All() => Ok(ApiResponse<List<OrderDto>>.Ok(await _orders.GetAllOrdersAsync()));
        [HttpPatch("{id}/status"), Authorize(Roles = "Admin")] public async Task<IActionResult> Status(int id, [FromBody] OrderStatus status) { await _orders.UpdateStatusAsync(id, status); return Ok(ApiResponse<string>.Ok("Status updated")); }
    }
}
