using RetailOrdering.API.DTOs;
using RetailOrdering.API.DTOs.Auth;
namespace RetailOrdering.API.Interfaces;

public interface IAuthService
{
    Task<TokenDto> RegisterAsync(RegisterDto dto);
    Task<TokenDto> LoginAsync(LoginDto dto);
    Task<TokenDto> RefreshAsync(string refreshToken);
}
