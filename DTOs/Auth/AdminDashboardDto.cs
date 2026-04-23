namespace RetailOrdering.API.DTOs.Auth
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int LowStockProducts { get; set; }
    }
}
