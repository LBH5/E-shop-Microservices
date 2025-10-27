
namespace CatalogAPI.Products.GetProducts
{
    public class GetProductsEndpoint : ICarterModule
    {
        public record GetProductsRequest(int? PageNumber = 1,int? PageSize = 10);
        public record GetProductsResponse(IEnumerable<Product> Products);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products",
                async([AsParameters] GetProductsRequest request,ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            })
             .WithName("GetProducts")
            .WithTags("Products")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Retrieves all products from the catalog.")
            .WithDescription("Retrieves a list of all products available in the catalog.");
        }
    }
}
