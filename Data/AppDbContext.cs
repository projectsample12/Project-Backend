using Microsoft.EntityFrameworkCore;
using RetailOrdering.API.Models;

namespace RetailOrdering.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Member 1
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // Member 2
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Packaging> Packagings { get; set; }

    // Member 3
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    // Member 4
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<DiscountCode> DiscountCodes { get; set; }
    public DbSet<LoyaltyPoint> LoyaltyPoints { get; set; }
    public DbSet<OrderHistory> OrderHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>().HasIndex(u => u.Email).IsUnique();
        mb.Entity<Order>().HasIndex(o => o.OrderNumber).IsUnique();
        mb.Entity<DiscountCode>().HasIndex(d => d.Code).IsUnique();

        mb.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
        mb.Entity<Product>().HasOne(p => p.Brand).WithMany(b => b.Products).HasForeignKey(p => p.BrandId);
        mb.Entity<CartItem>().HasOne(ci => ci.Cart).WithMany(c => c.Items).HasForeignKey(ci => ci.CartId);
        mb.Entity<CartItem>().HasOne(ci => ci.Product).WithMany().HasForeignKey(ci => ci.ProductId);
        mb.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId);
        mb.Entity<OrderItem>().HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductId);

        // ── Seed default Admin user ──────────────────────────────────────────
        mb.Entity<User>().HasData(new User
        {
            Id = 1,
            Name = "Super Admin",
            Email = "admin@retail.com",
            // BCrypt hash of "Admin@123"  (cost factor 11)
            PasswordHash = "$2b$11$BcMs25vqkjfJN6exz3YOpeyU1H4XDCcsxhJDXMvKH4XgW9U67XYkO",
            Role = "Admin",
            IsActive = true,
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}