namespace RetailOrdering.API.DTOs.Order
{
    public class OrderRequestDto
    {
        public string DeliveryAddress { get; set; } = string.Empty;
        public string? DiscountCode { get; set; }
        public bool UseRewardPoints { get; set; } = false; // ← add this
    }
}
