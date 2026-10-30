
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

    internal class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            var shoppingCart = command.ShoppingCart;

            await repository.StoreBasket(shoppingCart, cancellationToken);

            return new StoreBasketResult(command.ShoppingCart.UserName);
        }
    }
}
