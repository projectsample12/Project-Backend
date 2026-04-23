using RetailOrdering.API.Models;
namespace RetailOrdering.API.Interfaces;

public interface IPromotionService
{
    Task<List<Promotion>> GetActiveAsync();
    Task<Promotion> CreateAsync(Promotion promo);
    Task ExpireAsync(int id);
    Task<Promotion?> ValidatePromotionCodeAsync(string code);
}