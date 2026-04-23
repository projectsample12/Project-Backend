using RetailOrdering.API.DTOs.Order;
using RetailOrdering.API.Models;
namespace RetailOrdering.API.Interfaces;

public interface IOrderService
{
    Task<OrderConfirmationDto> PlaceOrderAsync(int userId, OrderRequestDto dto);
    Task<List<OrderDto>> GetUserOrdersAsync(int userId);
    Task<List<OrderDto>> GetAllOrdersAsync();
    Task UpdateStatusAsync(int orderId, OrderStatus status);
}