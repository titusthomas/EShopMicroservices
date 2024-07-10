
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string ImageFile, decimal Price):ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x=>x.Id).NotEmpty().WithMessage("Product Id required");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2,150).WithMessage("Name must be between 2 to 150 characters");
            
            
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price mustbe greater than 0");
        }

    }
    internal class UpdateProductCommandHandler(IDocumentSession session,ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"UpdateProduct {command}");

            var product=await session.LoadAsync<Product>(command.Id, cancellationToken);
            if(product is null)
            {
                throw new ProductNotFoundException();
            }

            product.Name = command.Name;
            product.Category = command.Category;
            product.ImageFile = command.ImageFile;
            product.Price= command.Price;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
