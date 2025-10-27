
namespace CatalogAPI.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id)
        : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    internal class GetProductByIdQueryHandler
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        private readonly IDocumentSession _session;

        public GetProductByIdQueryHandler(IDocumentSession session)
        {
            _session = session;
        }
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await this._session
                .LoadAsync<Product>(query.Id, cancellationToken);
            if (product == null) throw new ProductNotFoundException(query.Id);
            return new GetProductByIdResult(product);
        }
    }
}
