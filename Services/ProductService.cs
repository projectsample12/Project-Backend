using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.DTOs;
using RetailOrdering.API.DTOs.Product;
using RetailOrdering.API.Models;

namespace RetailOrdering.API.Services;

public class ProductService
{
    private readonly AppDbContext _db;
    public ProductService(AppDbContext db) => _db = db;

    public async Task<List<ProductDto>> GetAllAsync() =>
        await _db.Products.Where(p => p.IsActive)
            .Include(p => p.Category).Include(p => p.Brand)
            .Select(p => ToDto(p)).ToListAsync();

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var p = await _db.Products.Include(x => x.Category).Include(x => x.Brand)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Product not found");
        return ToDto(p);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var p = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId,
            BrandId = dto.BrandId,
            PackagingId = dto.PackagingId
        };
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        return await GetByIdAsync(p.Id);
    }

    public async Task<ProductDto> UpdateAsync(int id, CreateProductDto dto)
    {
        var p = await _db.Products.FindAsync(id) ?? throw new KeyNotFoundException();
        p.Name = dto.Name; p.Description = dto.Description; p.Price = dto.Price;
        p.Stock = dto.Stock; p.ImageUrl = dto.ImageUrl; p.CategoryId = dto.CategoryId;
        p.BrandId = dto.BrandId; p.PackagingId = dto.PackagingId;
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var p = await _db.Products.FindAsync(id) ?? throw new KeyNotFoundException();
        p.IsActive = false;
        await _db.SaveChangesAsync();
    }

    private static ProductDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Stock = p.Stock,
        ImageUrl = p.ImageUrl,
        CategoryName = p.Category?.Name ?? "",
        BrandName = p.Brand?.Name ?? ""
    };
}