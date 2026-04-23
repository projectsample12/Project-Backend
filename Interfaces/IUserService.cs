using RetailOrdering.API.DTOs;
using RetailOrdering.API.DTOs.User;
namespace RetailOrdering.API.Interfaces;

public interface IUserService
{
    Task<UserProfileDto> GetProfileAsync(int userId);
    Task UpdateProfileAsync(int userId, UpdateProfileDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
}