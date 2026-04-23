namespace RetailOrdering.API.Models
{
    public class Packaging
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty; // Box, Bottle, Bag...
        public string? Description { get; set; }
    }
}
