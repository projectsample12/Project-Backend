using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailOrdering.API.DTOs;
using RetailOrdering.API.DTOs.Auth;
using RetailOrdering.API.Helpers;
using RetailOrdering.API.Services;

namespace RetailOrdering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AdminService _admin;
    public AdminController(AdminService admin) => _admin = admin;

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard() =>
        Ok(ApiResponse<AdminDashboardDto>.Ok(await _admin.GetDashboardAsync()));

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers() =>
        Ok(ApiResponse<List<AdminUserDto>>.Ok(await _admin.GetUsersAsync()));

    [HttpPatch("users/{id}/activate")]
    public async Task<IActionResult> Activate(int id, [FromQuery] bool active)
    {
        await _admin.SetUserActiveAsync(id, active);
        return Ok(ApiResponse<string>.Ok("Updated"));
    }

    [HttpPatch("users/{id}/role")]
    public async Task<IActionResult> ChangeRole(int id, [FromBody] string role)
    {
        await _admin.ChangeUserRoleAsync(id, role);
        return Ok(ApiResponse<string>.Ok("Role updated"));
    }
}