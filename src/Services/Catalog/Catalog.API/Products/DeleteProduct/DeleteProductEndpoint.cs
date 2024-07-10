
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{
   // public record DeleteProductRequest(Guid id);
    public record DeleteProductResponse(bool IsSucess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{Id}",async (Guid Id, ISender sender) =>
            {
                var command=new DeleteProductCommand(Id);
                var result=await sender.Send(command);
                return Results.Ok( result.Adapt<DeleteProductResponse>());
            }).WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Product")
                .WithDescription("Delete Product");

        }
    }
}
