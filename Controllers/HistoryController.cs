using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetailOrdering.API.DTOs.Order;
using RetailOrdering.API.Helpers;
using RetailOrdering.API.Interfaces;
using RetailOrdering.API.Services;
using System.Security.Claims;

namespace RetailOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _hist;
        public HistoryController(IHistoryService hist) => _hist = hist;
        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet] public async Task<IActionResult> History() => Ok(ApiResponse<List<OrderDto>>.Ok(await _hist.GetHistoryAsync(UserId)));
    }
}
