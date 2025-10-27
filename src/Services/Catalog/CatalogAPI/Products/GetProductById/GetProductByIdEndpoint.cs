
namespace CatalogAPI.Products.GetProductById
{
    //public record GetProductByIdRequest(Guid Id);
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", 
                async ([FromRoute] Guid id,ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);

            })
            .WithName("GetProductById")
            .WithTags("Products")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Retrieves a product by its unique identifier.")
            .WithDescription("Retrieves the details of a specific product from the catalog using its unique identifier.");
        }
    }
}
