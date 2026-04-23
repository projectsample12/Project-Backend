using RetailOrdering.API.DTOs;
using RetailOrdering.API.DTOs.Auth;
namespace RetailOrdering.API.Interfaces;

public interface IAdminService
{
    Task<AdminDashboardDto> GetDashboardAsync();
    Task<List<AdminUserDto>> GetUsersAsync();          // was GetAllUsersAsync
    Task SetUserActiveAsync(int userId, bool active);  // was ActivateUserAsync
    Task ChangeUserRoleAsync(int userId, string role); // was UpdateUserRoleAsync
}