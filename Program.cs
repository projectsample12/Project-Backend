using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RetailOrdering.API.Data;
using RetailOrdering.API.Interfaces;
using RetailOrdering.API.Middleware;
using RetailOrdering.API.Services;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RetailOrdering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
            builder.Host.UseSerilog();

            // DB
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                ));

            // JWT
            var jwt = builder.Configuration.GetSection("JwtSettings");

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwt["SecretKey"]!))
                    };
                });
            builder.Services.AddAuthorization();

            // CORS
            builder.Services.AddCors(o => o.AddPolicy("Angular", p =>
                p.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!)
                 .AllowAnyMethod().AllowAnyHeader()));

            // DI — Member 1
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            // DI — Member 2
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<ISearchService, SearchService>();

            // DI — Member 3
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // DI — Member 4
            builder.Services.AddScoped<IPromotionService, PromotionService>();
            builder.Services.AddScoped<ILoyaltyService, LoyaltyService>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Swagger with JWT
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RetailOrdering API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
                    new OpenApiSecurityScheme { Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme, Id = "Bearer" }}, Array.Empty<string>()
                }});
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors("Angular");
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}