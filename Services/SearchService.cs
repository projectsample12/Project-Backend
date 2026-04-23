using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.DTOs.Product;
using RetailOrdering.API.Interfaces;

namespace RetailOrdering.API.Services
{
    public class SearchService : ISearchService
    {
        private readonly AppDbContext _db;
        public SearchService(AppDbContext db) => _db = db;
        public async Task<List<ProductDto>> SearchAsync(SearchFilterDto f)
        {
            var q = _db.Products.Where(p => p.IsActive).Include(p => p.Category).Include(p => p.Brand).AsQueryable();
            if (!string.IsNullOrEmpty(f.Keyword)) q = q.Where(p => p.Name.Contains(f.Keyword));
            if (f.CategoryId.HasValue) q = q.Where(p => p.CategoryId == f.CategoryId);
            if (f.BrandId.HasValue) q = q.Where(p => p.BrandId == f.BrandId);
            if (f.MinPrice.HasValue) q = q.Where(p => p.Price >= f.MinPrice);
            if (f.MaxPrice.HasValue) q = q.Where(p => p.Price <= f.MaxPrice);
            if (f.InStock == true) q = q.Where(p => p.Stock > 0);
            return await q.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name
            }).ToListAsync();
        }
    }
}
