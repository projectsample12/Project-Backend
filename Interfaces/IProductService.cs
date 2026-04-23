using RetailOrdering.API.DTOs.Product;
namespace RetailOrdering.API.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto> UpdateAsync(int id, CreateProductDto dto);
    Task DeleteAsync(int id);
}