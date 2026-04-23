using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Data;
using RetailOrdering.API.DTOs;
using RetailOrdering.API.DTOs.Auth;
using RetailOrdering.API.Interfaces;
using RetailOrdering.API.Models;

namespace RetailOrdering.API.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _db;
    public AdminService(AppDbContext db) => _db = db;

    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        return new AdminDashboardDto
        {
            TotalUsers = await _db.Users.CountAsync(),
            TotalOrders = await _db.Orders.CountAsync(),
            TotalRevenue = await _db.Orders.Where(o => o.Status == OrderStatus.Delivered).SumAsync(o => o.TotalAmount),
            LowStockProducts = await _db.Products.CountAsync(p => p.Stock < 10)
        };
    }

    public async Task<List<AdminUserDto>> GetUsersAsync() =>
        await _db.Users.Select(u => new AdminUserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            IsActive = u.IsActive
        }).ToListAsync();

    public async Task SetUserActiveAsync(int id, bool active)
    {
        var user = await _db.Users.FindAsync(id) ?? throw new KeyNotFoundException();
        user.IsActive = active;
        await _db.SaveChangesAsync();
    }

    public async Task ChangeUserRoleAsync(int id, string role)
    {
        var user = await _db.Users.FindAsync(id) ?? throw new KeyNotFoundException();
        user.Role = role;
        await _db.SaveChangesAsync();
    }
}