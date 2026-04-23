using RetailOrdering.API.Models;
namespace RetailOrdering.API.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}