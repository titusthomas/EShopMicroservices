

namespace Catalog.API.Products.GetProduct
{
    public record GetProductQuery() : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product> Products);
    internal class GetProductQueryHandler(IDocumentSession document, ILogger<GetProductQueryHandler> logger)
        : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Calling IQuery");
            var products = await document.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductResult(products);
        }
    }
}
