using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxTotalUsage",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxUsagePerUser",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MinOrderAmount",
                table: "Promotions",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalUsageCount",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxTotalUsage",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "MaxUsagePerUser",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "MinOrderAmount",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "TotalUsageCount",
                table: "Promotions");
        }
    }
}
