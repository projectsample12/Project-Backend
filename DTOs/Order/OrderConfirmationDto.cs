namespace RetailOrdering.API.DTOs.Order
{
    public class OrderConfirmationDto { public string OrderNumber { get; set; } = string.Empty; public string Message { get; set; } = string.Empty; public decimal TotalAmount { get; set; } }
}
