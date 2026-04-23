namespace RetailOrdering.API.Interfaces;

public interface ILoyaltyService
{
    Task<int> GetPointsAsync(int userId);
    Task AddPointsAsync(int userId, int points, string reason);
    Task DeductPointsAsync(int userId, int points, string reason);
}