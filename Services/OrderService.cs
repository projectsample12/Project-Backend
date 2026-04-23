using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.DTOs.Order;
using RetailOrdering.API.Models;

namespace RetailOrdering.API.Services
{
    public class OrderService
    {
        private readonly AppDbContext _db;
        private readonly CartService _cart;
        private readonly InventoryService _inventory;
        private readonly EmailService _email;
        private readonly LoyaltyService _loyalty;

        public OrderService(AppDbContext db, CartService cart, InventoryService inventory, EmailService email, LoyaltyService loyalty)
        { _db = db; _cart = cart; _inventory = inventory; _email = email; _loyalty = loyalty; }

        public async Task<OrderConfirmationDto> PlaceOrderAsync(int userId, OrderRequestDto dto)
        {
            var cart = await _db.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || !cart.Items.Any()) throw new ArgumentException("Cart is empty");

            var subtotal = cart.Items.Sum(i => i.Product.Price * i.Quantity);
            decimal discount = 0;

            if (!string.IsNullOrEmpty(dto.DiscountCode))
            {
                var promo = await _db.Promotions.FirstOrDefaultAsync(p =>
                    p.Code == dto.DiscountCode &&
                    p.IsActive &&
                    p.StartDate <= DateTime.UtcNow &&
                    p.EndDate >= DateTime.UtcNow &&
                    p.TotalUsageCount < p.MaxTotalUsage);

                if (promo != null)
                {
                    // constraint 1: minimum order amount
                    if (subtotal < promo.MinOrderAmount)
                        throw new ArgumentException(
                            $"Minimum order amount for this code is ₹{promo.MinOrderAmount}");

                    // constraint 2: one use per user
                    var alreadyUsed = await _db.Orders.AnyAsync(o =>
                        o.UserId == userId &&
                        o.DiscountCode == dto.DiscountCode);

                    if (alreadyUsed)
                        throw new ArgumentException("You have already used this discount code");

                    discount = subtotal * (promo.DiscountPercent / 100);
                    promo.TotalUsageCount++;
                }
                else
                {
                    // fallback DiscountCodes table
                    var code = await _db.DiscountCodes.FirstOrDefaultAsync(d =>
                        d.Code == dto.DiscountCode &&
                        d.IsActive &&
                        d.ExpiresAt > DateTime.UtcNow &&
                        d.UsageCount < d.MaxUsage);

                    if (code != null)
                    {
                        discount = code.DiscountAmount;
                        code.UsageCount++;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid or expired discount code");
                    }
                }
            }

            // loyalty points redemption
            if (dto.UseRewardPoints)
            {
                int userPoints = await _loyalty.GetPointsAsync(userId);
                if (userPoints > 0)
                {
                    decimal maxPointsDiscount = subtotal * 0.10m;
                    decimal pointsDiscount = Math.Min(userPoints, maxPointsDiscount);
                    discount += pointsDiscount;
                    await _loyalty.DeductPointsAsync(userId, (int)pointsDiscount,
                        "Redeemed for order");
                }
            }

            var total = subtotal - discount;
            if (total < 0) total = 0;

            var order = new Order
            {
                OrderNumber = "ORD-" + DateTime.UtcNow.Ticks,
                UserId = userId,
                DeliveryAddress = dto.DeliveryAddress,
                DiscountCode = dto.DiscountCode,
                DiscountAmount = discount,
                TotalAmount = total,
                Status = OrderStatus.Confirmed,
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.Price
                }).ToList()
            };

            _db.Orders.Add(order);

            foreach (var item in cart.Items)
                await _inventory.DeductStockAsync(item.ProductId, item.Quantity);

            await _cart.ClearCartAsync(userId);
            await _db.SaveChangesAsync();

            // earn loyalty points — 1 point per ₹100 spent
            int pointsEarned = (int)(total / 100);
            if (pointsEarned > 0)
                await _loyalty.AddPointsAsync(userId, pointsEarned,
                    $"Earned for order {order.OrderNumber}");

            var user = await _db.Users.FindAsync(userId)!;
            await _email.SendOrderConfirmationAsync(user!.Email, order.OrderNumber, total);

            return new OrderConfirmationDto
            {
                OrderNumber = order.OrderNumber,
                TotalAmount = total,
                Message = $"Order placed successfully! Discount applied: ₹{discount:F2}. Points earned: {pointsEarned}. Confirmation sent to email."
            };
        }

        public async Task<List<OrderDto>> GetUserOrdersAsync(int userId) =>
            await _db.Orders.Where(o => o.UserId == userId)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .Select(o => MapOrder(o)).ToListAsync();

        public async Task<List<OrderDto>> GetAllOrdersAsync() =>
            await _db.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                .Select(o => MapOrder(o)).ToListAsync();

        public async Task UpdateStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _db.Orders.FindAsync(orderId) ?? throw new KeyNotFoundException();
            order.Status = status;
            await _db.SaveChangesAsync();
        }

        private static OrderDto MapOrder(Order o) => new()
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            Status = o.Status.ToString(),
            TotalAmount = o.TotalAmount,
            DiscountAmount = o.DiscountAmount,
            DeliveryAddress = o.DeliveryAddress,
            CreatedAt = o.CreatedAt,
            Items = o.Items.Select(i => new OrderItemDto
            {
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Subtotal = i.UnitPrice * i.Quantity
            }).ToList()
        };
    }
}