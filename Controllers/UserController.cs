using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetailOrdering.API.DTOs.User;
using RetailOrdering.API.Helpers;
using RetailOrdering.API.Interfaces;
using RetailOrdering.API.Services;
using System.Security.Claims;

namespace RetailOrdering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _users;
        public UserController(IUserService users) => _users = users;
        private int UserId => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
     ? id
     : throw new UnauthorizedAccessException("Invalid or missing token.");

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile() =>
            Ok(ApiResponse<UserProfileDto>.Ok(await _users.GetProfileAsync(UserId)));

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            await _users.UpdateProfileAsync(UserId, dto);
            return Ok(ApiResponse<string>.Ok("Profile updated"));
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            await _users.ChangePasswordAsync(UserId, dto);
            return Ok(ApiResponse<string>.Ok("Password changed successfully"));
        }
    }
}
