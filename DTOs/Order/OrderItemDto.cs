namespace RetailOrdering.API.DTOs.Order
{
    public class OrderItemDto { public string ProductName { get; set; } = string.Empty; public int Quantity { get; set; } public decimal UnitPrice { get; set; } public decimal Subtotal { get; set; } }
}
