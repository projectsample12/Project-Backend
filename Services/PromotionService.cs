using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.Interfaces;
using RetailOrdering.API.Models;

namespace RetailOrdering.API.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly AppDbContext _db;
        public PromotionService(AppDbContext db) => _db = db;

        public async Task<List<Promotion>> GetActiveAsync() =>
            await _db.Promotions.Where(p => p.IsActive && p.EndDate >= DateTime.UtcNow).ToListAsync();

        public async Task<Promotion> CreateAsync(Promotion promo)
        {
            _db.Promotions.Add(promo); await _db.SaveChangesAsync(); return promo;
        }

        public async Task ExpireAsync(int id)
        {
            var p = await _db.Promotions.FindAsync(id) ?? throw new KeyNotFoundException();
            p.IsActive = false; await _db.SaveChangesAsync();
        }

        public async Task<DiscountCode?> ValidateCodeAsync(string code) =>
            await _db.DiscountCodes.FirstOrDefaultAsync(d =>
                d.Code == code && d.IsActive && d.ExpiresAt > DateTime.UtcNow && d.UsageCount < d.MaxUsage);

        // ✅ New — validates promotion by Code field
        public async Task<Promotion?> ValidatePromotionCodeAsync(string code) =>
            await _db.Promotions.FirstOrDefaultAsync(p =>
                p.Code == code && p.IsActive && p.EndDate >= DateTime.UtcNow);
    }
}