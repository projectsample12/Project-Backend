namespace RetailOrdering.API.Models
{
    public class DiscountCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public bool IsActive { get; set; } = true;
        public int MaxUsage { get; set; }
        public int UsageCount { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
