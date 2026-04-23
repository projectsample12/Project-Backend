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
    public class ProductController : ControllerBase
    {
        private readonly ProductService _products;
        private readonly SearchService _search;
        public ProductController(ProductService products, SearchService search)
        { _products = products; _search = search; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(ApiResponse<List<ProductDto>>.Ok(await _products.GetAllAsync()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) =>
            Ok(ApiResponse<ProductDto>.Ok(await _products.GetByIdAsync(id)));

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchFilterDto f) =>
            Ok(ApiResponse<List<ProductDto>>.Ok(await _search.SearchAsync(f)));

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductDto dto) =>
            Ok(ApiResponse<ProductDto>.Ok(await _products.CreateAsync(dto)));

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, CreateProductDto dto) =>
            Ok(ApiResponse<ProductDto>.Ok(await _products.UpdateAsync(id, dto)));

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        { await _products.DeleteAsync(id); return Ok(ApiResponse<string>.Ok("Deleted")); }
    }
}
