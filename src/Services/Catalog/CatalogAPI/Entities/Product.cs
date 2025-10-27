namespace CatalogAPI.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; }= default!;
        public List<string> Category { get; set; } = new List<string>();
        public string ImageFile { get; set; } = default!;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

    }
}
