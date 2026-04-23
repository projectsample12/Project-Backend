using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.Helpers;
using RetailOrdering.API.Models;
using RetailOrdering.API.Services;
using System.Security.Claims;

namespace RetailOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly PromotionService _promos;
        private readonly LoyaltyService _loyalty;
        private readonly AppDbContext _db;

        public PromotionController(PromotionService promos, LoyaltyService loyalty, AppDbContext db)
        {
            _promos = promos;
            _loyalty = loyalty;
            _db = db;
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> Active() =>
            Ok(ApiResponse<List<Promotion>>.Ok(await _promos.GetActiveAsync()));

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Promotion p) =>
            Ok(ApiResponse<Promotion>.Ok(await _promos.CreateAsync(p)));

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Expire(int id)
        {
            await _promos.ExpireAsync(id);
            return Ok(ApiResponse<string>.Ok("Expired"));
        }

        [HttpGet("validate/{code}")]
        public async Task<IActionResult> Validate(string code)
        {
            var promo = await _promos.ValidatePromotionCodeAsync(code);
            if (promo is null)
                return BadRequest(new { success = false, message = "Invalid or expired code.", data = (object?)null });
            return Ok(new { success = true, message = "Valid promotion.", data = promo });
        }

        // ← NEW: Admin creates a usable discount code
        [HttpPost("discount-code"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDiscountCode(DiscountCode dto)
        {
            _db.DiscountCodes.Add(dto);
            await _db.SaveChangesAsync();
            return Ok(ApiResponse<DiscountCode>.Ok(dto));
        }

        // ← NEW: Get all discount codes (Admin)
        [HttpGet("discount-codes"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDiscountCodes() =>
            Ok(ApiResponse<List<DiscountCode>>.Ok(await _db.DiscountCodes.ToListAsync()));

        [HttpGet("loyalty"), Authorize]
        public async Task<IActionResult> Points() =>
            Ok(ApiResponse<int>.Ok(await _loyalty.GetPointsAsync(UserId)));
    }
}