using RetailOrdering.API.DTOs.Order;
namespace RetailOrdering.API.Interfaces;

public interface ICartService
{
    Task<CartDto> GetCartAsync(int userId);
    Task AddToCartAsync(int userId, CartItemDto dto);
    Task RemoveFromCartAsync(int userId, int productId);
    Task ClearCartAsync(int userId);
}