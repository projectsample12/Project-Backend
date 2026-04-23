using RetailOrdering.API.DTOs.Product;
namespace RetailOrdering.API.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto> CreateAsync(CategoryDto dto);
}