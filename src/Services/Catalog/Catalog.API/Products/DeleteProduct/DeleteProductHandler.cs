﻿
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid id):ICommand<DeleteProductResult>;
    public record  DeleteProductResult(bool IsSucess);
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Product Id required");
        }

    }
    internal class DeleteProductCommandHandler(IDocumentSession session,ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"DeleteProductCommand handler called with {command}");
            session.Delete<Product>(command.id);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);
        }
    }
}
