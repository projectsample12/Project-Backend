using RetailOrdering.API.DTOs.Product;
namespace RetailOrdering.API.Interfaces;

public interface ISearchService
{
    Task<List<ProductDto>> SearchAsync(SearchFilterDto filter);
}