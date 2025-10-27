using Marten.Schema;

namespace CatalogAPI.Data
{
    public class CatalogInitialData : IInitialData
    {

        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            var session = store.LightweightSession();
            if(await session.Query<Product>().AnyAsync(cancellation))
            {
                return;
            }
            session.Store<Product>(GetPreConfiguredProducts());
            await session.SaveChangesAsync(cancellation);
        }

        private static IEnumerable<Product> GetPreConfiguredProducts()
        {
            return new List<Product>()
            {
                new Product
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "IPhone 13",
                    Description = "IPhone 13 Description",
                    Category = new List<string>() { "Smart Phone" },
                    ImageFile = "iphone13.png",
                    Price = 999.99M
                },
                new Product
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Samsung Galaxy S21",
                    Description = "Samsung Galaxy S21 Description",
                    Category = new List<string>() { "Smart Phone" },
                    ImageFile = "galaxys21.png",
                    Price = 899.99M
                },
                new Product
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Google Pixel 6",
                    Description = "Google Pixel 6 Description",
                    Category = new List<string>() { "Smart Phone" },
                    ImageFile = "pixel6.png",
                    Price = 799.99M
                },
                new Product
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "OnePlus 9",
                    Description = "OnePlus 9 Description",
                    Category = new List<string>() { "Smart Phone" },
                    ImageFile = "oneplus9.png",
                    Price = 699.99M
                },
                new Product
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Sony WH-1000XM4",
                    Description = "Sony WH-1000XM4 Description",
                    Category = new List<string>() { "Headphones" },
                    ImageFile = "sonywh1000xm4.png",
                    Price = 349.99M
                },
                new Product
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "Bose QuietComfort 35 II",
                    Description = "Bose QuietComfort 35 II Description",
                    Category = new List<string>() { "Headphones" },
                    ImageFile = "boseqc35ii.png",
                    Price = 299.99M
                },
                new Product
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Name = "Apple MacBook Pro",
                    Description = "Apple MacBook Pro Description",
                    Category = new List<string>() { "Laptops" },
                    ImageFile = "macbookpro.png",
                    Price = 1999.99M
                },
            };
        }
    }
}
