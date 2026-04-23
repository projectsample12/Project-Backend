using RetailOrdering.API.DTOs.Order;
namespace RetailOrdering.API.Interfaces;

public interface IHistoryService
{
    Task<List<OrderDto>> GetHistoryAsync(int userId);
}