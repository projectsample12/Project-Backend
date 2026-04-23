public class Promotion
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }

    // ← Add these constraints
    public decimal MinOrderAmount { get; set; } = 0;      // min cart value to apply
    public int MaxUsagePerUser { get; set; } = 1;         // one use per user
    public int TotalUsageCount { get; set; } = 0;         // how many times used total
    public int MaxTotalUsage { get; set; } = 100;         // max total uses
}