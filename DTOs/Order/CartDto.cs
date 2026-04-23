namespace RetailOrdering.API.DTOs.Order
{
    public class CartDto { public int Id { get; set; } public List<CartItemDetailDto> Items { get; set; } = new(); public decimal Total { get; set; } }
}
