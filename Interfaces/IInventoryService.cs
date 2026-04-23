using RetailOrdering.API.DTOs.Product;
using RetailOrdering.API.Models;
namespace RetailOrdering.API.Interfaces;

public interface IInventoryService
{
    Task<List<InventoryDto>> GetInventoryAsync();
    Task UpdateStockAsync(StockUpdateDto dto);
    Task DeductStockAsync(int productId, int quantity);
    Task<List<Brand>> GetBrandsAsync();
    Task<Brand> CreateBrandAsync(BrandDto dto);
    Task<Brand> UpdateBrandAsync(int id, BrandDto dto);
    Task DeleteBrandAsync(int id);
}