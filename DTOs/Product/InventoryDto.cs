namespace RetailOrdering.API.DTOs.Product
{
    // DTOs/Product/InventoryDto.cs
    public class InventoryDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string BrandName { get; set; } = string.Empty;   // ← add this
        public string CategoryName { get; set; } = string.Empty; // ← add this
    }
}
