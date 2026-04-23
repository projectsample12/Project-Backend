using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.DTOs.Product;
using RetailOrdering.API.Interfaces;
using RetailOrdering.API.Models;

namespace RetailOrdering.API.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _db;
        public InventoryService(AppDbContext db) => _db = db;

        // existing methods
        public async Task<List<InventoryDto>> GetInventoryAsync() =>
    await _db.Products
        .Where(p => p.IsActive)
        .Include(p => p.Brand)       // ← add
        .Include(p => p.Category)    // ← add
        .Select(p => new InventoryDto
        {
            ProductId = p.Id,
            ProductName = p.Name,
            Stock = p.Stock,
            BrandName = p.Brand.Name,       // ← add
            CategoryName = p.Category.Name  // ← add
        })
        .ToListAsync();

        public async Task UpdateStockAsync(StockUpdateDto dto)
        {
            var p = await _db.Products.FindAsync(dto.ProductId)
                ?? throw new KeyNotFoundException();
            p.Stock = dto.NewStock;
            await _db.SaveChangesAsync();
        }

        // new brand methods
        public async Task<List<Brand>> GetBrandsAsync() =>
            await _db.Brands.ToListAsync();

        public async Task<Brand> CreateBrandAsync(BrandDto dto)
        {
            var brand = new Brand { Name = dto.Name };
            _db.Brands.Add(brand);
            await _db.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand> UpdateBrandAsync(int id, BrandDto dto)
        {
            var brand = await _db.Brands.FindAsync(id)
                ?? throw new KeyNotFoundException("Brand not found");
            brand.Name = dto.Name;
            await _db.SaveChangesAsync();
            return brand;
        }

        public async Task DeleteBrandAsync(int id)
        {
            var brand = await _db.Brands.FindAsync(id)
                ?? throw new KeyNotFoundException("Brand not found");
            _db.Brands.Remove(brand);
            await _db.SaveChangesAsync();
        }

        public async Task DeductStockAsync(int productId, int quantity)
        {
            var product = await _db.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException("Product not found");

            if (product.Stock < quantity)
                throw new InvalidOperationException($"Insufficient stock for product '{product.Name}'");

            product.Stock -= quantity;
            await _db.SaveChangesAsync();
        }
    }
}
