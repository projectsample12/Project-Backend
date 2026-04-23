using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.Interfaces;
using RetailOrdering.API.Models;

namespace RetailOrdering.API.Services
{
    public class LoyaltyService : ILoyaltyService
    {
        private readonly AppDbContext _db;
        public LoyaltyService(AppDbContext db) => _db = db;

        public async Task<int> GetPointsAsync(int userId) =>
            await _db.LoyaltyPoints.Where(l => l.UserId == userId).SumAsync(l => l.Points);

        public async Task AddPointsAsync(int userId, int points, string reason)
        {
            _db.LoyaltyPoints.Add(new LoyaltyPoint { UserId = userId, Points = points, Reason = reason });
            await _db.SaveChangesAsync();
        }

        public async Task DeductPointsAsync(int userId, int points, string reason)
        {
            _db.LoyaltyPoints.Add(new LoyaltyPoint
            {
                UserId = userId,
                Points = -points,    // negative = deduction
                Reason = reason
            });
            await _db.SaveChangesAsync();
        }
    }
}
