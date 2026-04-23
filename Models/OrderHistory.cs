namespace RetailOrdering.API.Models
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    }
}
