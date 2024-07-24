
using Mapster;

namespace Basket.API.Basket.GetBasket
{

    //public record GetBasketRequest(string UserName) 
    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var response = await sender.Send(new GetBasketQuery(userName));
                return Results.Ok(response.Adapt<GetBasketResponse>());
            }).WithName("GetBasket")
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Basket")
                .WithDescription("Get Basket");
        }
    }
}
