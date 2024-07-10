
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductsByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}",async (string category,ISender sender) =>
            {
                var result = await sender.Send(new GetProductsByCategoryQuery(category));
                return Results.Ok(result.Adapt<GetProductsByCategoryResponse>());
            }).WithName("GetProductsByCategory")
                .Produces<CreateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product by Category")
                .WithDescription("Get Product by Category");

        }
    }
}
