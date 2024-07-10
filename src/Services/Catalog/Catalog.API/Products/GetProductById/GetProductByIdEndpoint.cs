
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProductById
{
    //public record GetProductByIDRequest();
    public record GetProductByIDResponse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{Id}",async (Guid Id,ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(Id));
                var response = result.Adapt<GetProductByIDResponse>();
                return Results.Ok( response);
            }).WithName("GetProductById")
                .Produces<CreateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product by id")
                .WithDescription("Get Product by id");
        }
    }
}
