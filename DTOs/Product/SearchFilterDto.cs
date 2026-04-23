namespace RetailOrdering.API.DTOs.Product
{
    public class SearchFilterDto
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStock { get; set; }
    }
}
