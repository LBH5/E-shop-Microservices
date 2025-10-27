
namespace CatalogAPI.Products.UpdateProduct
{
    public record UpdateProductCommand(
            Guid Id,
            string Name,
            string Description,
            List<string> Category,
            string ImageFile,
            decimal Price
        ) 
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator
        : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product Id is required!");
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product Name is required!")
                .Length(5, 100)
                .WithMessage("Product Name must be between 5 and 100 characters!")
                .When(x => x.Name != null);
            RuleFor(x => x.ImageFile)
                .NotEmpty()
                .WithMessage("Product Image File is required!")
                .When(x => x.ImageFile != null);
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Product Price must be greater than 0")
                .When(x => x.Price != 0);
        }
    }
    internal class UpdateProductCommandHandler :
        ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {

        private readonly IDocumentSession _session;


        public UpdateProductCommandHandler(IDocumentSession session)
        {
            _session=session;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await this._session
                .LoadAsync<Product>(command.Id, cancellationToken);
            if (product == null) throw new ProductNotFoundException(command.Id);
            if(command.Name != null) product.Name = command.Name;
            if(command.Description != null) product.Description = command.Description;
            if(command.Category != null) product.Category = command.Category;
            if(command.ImageFile != null) product.ImageFile = command.ImageFile;
            if(command.Price != 0) product.Price = command.Price;
            product.UpdatedAt = DateTime.UtcNow;
            this._session.Update(product);
            await this._session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }
    }
}
