namespace RetailOrdering.API.DTOs.Order
{
    public class CartItemDetailDto { public int ProductId { get; set; } public string ProductName { get; set; } = string.Empty; public decimal Price { get; set; } public int Quantity { get; set; } public decimal Subtotal { get; set; } }
}
