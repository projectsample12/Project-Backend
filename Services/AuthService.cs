using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.DTOs;
using RetailOrdering.API.DTOs.Auth;
using RetailOrdering.API.Interfaces;
using RetailOrdering.API.Models;

namespace RetailOrdering.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, JwtService jwt, IConfiguration config)
    { _db = db; _jwt = jwt; _config = config; }

    public async Task<TokenDto> RegisterAsync(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            throw new ArgumentException("Email already registered.");
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return await IssueTokens(user);
    }

    public async Task<TokenDto> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");
        if (!user.IsActive) throw new UnauthorizedAccessException("Account is deactivated.");
        return await IssueTokens(user);
    }

    public async Task<TokenDto> RefreshAsync(string refreshToken)
    {
        var days = int.Parse(_config["JwtSettings:RefreshTokenExpirationDays"]!);
        var token = await _db.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
            ?? throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        token.IsRevoked = true;
        await _db.SaveChangesAsync();
        return await IssueTokens(token.User);
    }

    private async Task<TokenDto> IssueTokens(User user)
    {
        var days = int.Parse(_config["JwtSettings:RefreshTokenExpirationDays"]!);
        var rt = new RefreshToken
        {
            Token = _jwt.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(days),
            UserId = user.Id
        };
        _db.RefreshTokens.Add(rt);
        await _db.SaveChangesAsync();
        return new TokenDto { AccessToken = _jwt.GenerateAccessToken(user), RefreshToken = rt.Token, Role = user.Role };
    }
}
