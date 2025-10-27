
namespace CatalogAPI.Products.GetProductByCategory
{
    //public record GetProductByCategoryRequest(string Category)
    //    : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}",
                async ([FromRoute] string category, ISender sender) =>
            {
                var query = new GetProductByCategoryQuery(category);
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductByCategory")
            .WithTags("Products")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Retrieves products by category from the catalog.")
            .WithDescription("Retrieves a list of products that belong to the specified category from the catalog.");
        }
    }
}
