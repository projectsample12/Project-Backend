using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetailOrdering.API.DTOs.Product;
using RetailOrdering.API.Helpers;
using RetailOrdering.API.Services;

namespace RetailOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _cats;
        public CategoryController(CategoryService cats) => _cats = cats;
        [HttpGet] public async Task<IActionResult> GetAll() => Ok(ApiResponse<List<CategoryDto>>.Ok(await _cats.GetAllAsync()));
        [HttpPost, Authorize(Roles = "Admin")] public async Task<IActionResult> Create(CategoryDto dto) => Ok(ApiResponse<CategoryDto>.Ok(await _cats.CreateAsync(dto)));
    }
}
