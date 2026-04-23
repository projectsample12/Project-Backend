using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.DTOs.Product;
using RetailOrdering.API.Models;

namespace RetailOrdering.API.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _db;
        public CategoryService(AppDbContext db) => _db = db;
        public async Task<List<CategoryDto>> GetAllAsync() =>
            await _db.Categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name, Description = c.Description }).ToListAsync();
        public async Task<CategoryDto> CreateAsync(CategoryDto dto)
        {
            var c = new Category { Name = dto.Name, Description = dto.Description };
            _db.Categories.Add(c); await _db.SaveChangesAsync();
            dto.Id = c.Id; return dto;
        }
    }
}
