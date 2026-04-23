using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailOrdering.API.DTOs;
using RetailOrdering.API.DTOs.Product;
using RetailOrdering.API.Helpers;
using RetailOrdering.API.Interfaces;
using RetailOrdering.API.Models;
using RetailOrdering.API.Services;

namespace RetailOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IInventoryService _inv;
        public BrandController(IInventoryService inv) => _inv = inv;

        [HttpGet("inventory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInventory() =>
            Ok(ApiResponse<List<InventoryDto>>.Ok(await _inv.GetInventoryAsync()));

        [HttpPatch("stock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStock(StockUpdateDto dto)
        {
            await _inv.UpdateStockAsync(dto);
            return Ok(ApiResponse<string>.Ok("Stock updated"));
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands() =>
            Ok(ApiResponse<List<Brand>>.Ok(await _inv.GetBrandsAsync()));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBrand(BrandDto dto) =>
            Ok(ApiResponse<Brand>.Ok(await _inv.CreateBrandAsync(dto)));

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBrand(int id, BrandDto dto) =>
            Ok(ApiResponse<Brand>.Ok(await _inv.UpdateBrandAsync(id, dto)));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            await _inv.DeleteBrandAsync(id);
            return Ok(ApiResponse<string>.Ok("Brand deleted"));
        }
    }
}