

using Marten.Pagination;

namespace Catalog.API.Products.GetProduct
{
    public record GetProductQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product> Products);
    internal class GetProductQueryHandler(IDocumentSession document)
        : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            
            var products = await document.Query<Product>().ToPagedListAsync(request.PageNumber??1,request.PageSize??10, cancellationToken);
            return new GetProductResult(products);
        }
    }
}
