using Microsoft.AspNetCore.Mvc;
using RetailOrdering.API.DTOs;
using RetailOrdering.API.DTOs.Auth;
using RetailOrdering.API.Helpers;
using RetailOrdering.API.Services;

namespace RetailOrdering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    public AuthController(AuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto) =>
        Ok(ApiResponse<TokenDto>.Ok(await _auth.RegisterAsync(dto)));

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto) =>
        Ok(ApiResponse<TokenDto>.Ok(await _auth.LoginAsync(dto)));

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken) =>
        Ok(ApiResponse<TokenDto>.Ok(await _auth.RefreshAsync(refreshToken)));
}