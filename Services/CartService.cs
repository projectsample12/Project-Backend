using RetailOrdering.API.Data;
using RetailOrdering.API.DTOs.Order;
using RetailOrdering.API.Models;
using Microsoft.EntityFrameworkCore;

namespace RetailOrdering.API.Services
{
    public class CartService
    {
        private readonly AppDbContext _db;
        public CartService(AppDbContext db) => _db = db;

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _db.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null) { cart = new Cart { UserId = userId }; _db.Carts.Add(cart); await _db.SaveChangesAsync(); }
            return cart;
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            return new CartDto
            {
                Id = cart.Id,
                Items = cart.Items.Select(i => new CartItemDetailDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Price = i.Product.Price,
                    Quantity = i.Quantity,
                    Subtotal = i.Product.Price * i.Quantity
                }).ToList(),
                Total = cart.Items.Sum(i => i.Product.Price * i.Quantity)
            };
        }

        public async Task AddToCartAsync(int userId, CartItemDto dto)
        {
            var product = await _db.Products.FindAsync(dto.ProductId) ?? throw new KeyNotFoundException("Product not found");
            if (product.Stock < dto.Quantity) throw new ArgumentException("Insufficient stock");
            var cart = await GetOrCreateCartAsync(userId);
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existing != null) existing.Quantity += dto.Quantity;
            else cart.Items.Add(new CartItem { CartId = cart.Id, ProductId = dto.ProductId, Quantity = dto.Quantity });
            cart.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null) { _db.CartItems.Remove(item); await _db.SaveChangesAsync(); }
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            _db.CartItems.RemoveRange(cart.Items);
            await _db.SaveChangesAsync();
        }
    }
}
