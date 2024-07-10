﻿using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryQuery(string category):IQuery<GetProductsByCategoryResult>;
    public record GetProductsByCategoryResult(IEnumerable<Product> Products);
    public class GetProductsByCategoryQueryHandler (IDocumentSession session,ILogger<GetProductsByCategoryQueryHandler> logger)
        : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Calling Get Products By Category @Query: {query}");
            var result=await session.Query<Product>().Where(p=>p.Category.Contains(  query.category)).ToListAsync(cancellationToken);
            return new GetProductsByCategoryResult(result);
        }
    }
}