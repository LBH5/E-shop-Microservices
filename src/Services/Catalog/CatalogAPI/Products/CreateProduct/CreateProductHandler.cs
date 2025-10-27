



namespace CatalogAPI.Products.CreateProduct
{
    public record CreateProductCommand(string Name,List<string> Category,string Description,string ImageFile,decimal Price)
        :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product Name is required!")
                .Length(5, 100)
                .WithMessage("Product Name must be between 5 and 100 characters!");
            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Product Category is required!");
            RuleFor(x => x.ImageFile)
                .NotEmpty()
                .WithMessage("Product Image File is required!");
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Product Price must be greater than 0");

        }
    }
    internal class CreateProductCommandHandler : 
        ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IDocumentSession _session;

        public CreateProductCommandHandler(IDocumentSession session)
        {
            this._session = session;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Validate the command
            // 1 - create product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };
            // 2 - save entity to database
            this._session.Store(product);
            await this._session.SaveChangesAsync(cancellationToken);
            // 3 - return created product id as result
            return new CreateProductResult(product.Id);
        }
    }

}
