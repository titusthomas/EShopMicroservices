
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.GetProductsByCategory;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string ImageFile, decimal Price) : ICommand<UpdateProductResponse>;
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest reuest, ISender sender) =>
            {
                var product = reuest.Adapt<UpdateProductCommand>();
                var result = await sender.Send(product);
                return Results.Ok(result.Adapt<UpdateProductResponse>());
            }).WithName("UpdateProduct")
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Update Product")
                .WithDescription("Update Product");

        }
    }
}
