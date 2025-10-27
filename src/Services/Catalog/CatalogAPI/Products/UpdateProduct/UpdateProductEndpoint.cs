
namespace CatalogAPI.Products.UpdateProduct
{
    public class UpdateProductEndpoint : ICarterModule
    {
        public record UpdateProductRequest(
            Guid Id,
            string Name,
            string Description,
            List<string> Category,
            string ImageFile,
            decimal Price
        );
        public record UpdateProductResponse(bool IsSuccess);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/products/{id}",
                async (
                    [FromRoute] Guid id,
                    [FromBody] UpdateProductRequest request,
                      ISender sender) =>
                {
                    var command = request.Adapt<UpdateProductCommand>() with { Id = id};
                    var result = await sender.Send(command);
                    var response = result.Adapt<UpdateProductResponse>();
                    return Results.Ok(response);

                })
                .WithName("UpdateProduct")
                .WithTags("Products")
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Updates an existing product in the catalog.")
                .WithDescription("Updates the details of an existing product in the catalog using the provided information.");
        }
    }
}
