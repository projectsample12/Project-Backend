using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.DTOs.Order;

namespace RetailOrdering.API.Services
{
    public class HistoryService
    {
        private readonly AppDbContext _db;
        public HistoryService(AppDbContext db) => _db = db;

        public async Task<List<OrderDto>> GetHistoryAsync(int userId) =>
            await _db.Orders.Where(o => o.UserId == userId).OrderByDescending(o => o.CreatedAt)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Status = o.Status.ToString(),
                    TotalAmount = o.TotalAmount,
                    CreatedAt = o.CreatedAt,
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Subtotal = i.UnitPrice * i.Quantity
                    }).ToList()
                }).ToListAsync();
    }
}
