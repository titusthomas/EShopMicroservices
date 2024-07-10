﻿
namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid id):IQuery<GetProductByIdResult>;
    
    public record GetProductByIdResult(Product Product);
    internal class GetProductByIdHandler(IDocumentSession session,ILogger<GetProductByIdHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Fetching data of @Query:{query}");
            var product = await session.LoadAsync<Product>(query.id);
            if( product is null ) 
            {
                throw new ProductNotFoundException();
            }

            return new GetProductByIdResult(product);
        }
    }
}
