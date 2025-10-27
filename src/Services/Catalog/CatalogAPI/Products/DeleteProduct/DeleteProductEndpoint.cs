
namespace CatalogAPI.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Guid)
    //    :ICommand<DeleteProductResponse>;
    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}",
                async ([FromRoute] Guid id, ISender sender) =>
            {
                var command = new DeleteProductCommand(id);
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteProductResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteProduct")
            .WithTags("Products")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Deletes a product from the catalog.")
            .WithDescription("Deletes a specific product from the catalog using its unique identifier.");
        }
    }
}
