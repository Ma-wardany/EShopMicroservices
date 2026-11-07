
using Discount.gRPC;
using System.Threading.Tasks;

namespace Basket.API.Basket.StoreBasket
{

    public record StoreBasketCommand(ShoppingCart ShoppingCart) : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.ShoppingCart).NotNull().WithMessage("Shopping cart cannot be null.");
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("User name cannot be empty.");
            RuleFor(x => x.ShoppingCart.Items).NotNull().WithMessage("Shopping cart items cannot be null.");

        }
    }

    internal class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto) 
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {

            await _applyDiscount(command.ShoppingCart, cancellationToken);

            await repository.StoreBasket(command.ShoppingCart, cancellationToken);

            return new StoreBasketResult(command.ShoppingCart.UserName);
        }

        private async Task _applyDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var request = new GetDiscountRequest { ProductName = item.ProductName };
                var coupon = await discountProto.GetDiscountAsync(request);

                item.Price -= coupon.Amount;
            }
        }
    }
}
