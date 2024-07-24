


namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart):ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username required");
        }
    }
    public class StoreBasketCommandHandler(IBasketRepository repository,Discount.Grpc.Protos.DiscountProtoService.DiscountProtoServiceClient discount) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.Cart,cancellationToken:cancellationToken);
            //ShoppingCart cart = command.Cart;
            var response=await repository.StoreBasket(command.Cart, cancellationToken);
            return new StoreBasketResult(response.UserName);
        }
        public async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discount.GetDiscountAsync(new Discount.Grpc.Protos.GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                if (coupon != null && coupon.Amount > 0)
                {
                    item.Price -= coupon.Amount;
                }
            }
        }
    }

}
