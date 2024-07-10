
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProduct
{
    public record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products",async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductQuery());
                var response = result.Adapt<GetProductResponse>();
                return Results.Ok(result);
            }).WithName("GetProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product")
                .WithDescription("Get Product");
        }
    }
}
