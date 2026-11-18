using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.Checkout
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) 
        : ICommand<CheckoutBasketCommandResult>;

    public record CheckoutBasketCommandResult(bool IsSuccess);

    public class CheckoutBasketCommandValidator
    : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
            RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class CheckoutBasketHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketCommandResult>
    {
        public async Task<CheckoutBasketCommandResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasket(command.BasketCheckoutDto.UserName);

            if (basket == null)
                return new CheckoutBasketCommandResult(false);
            
            var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            await publishEndpoint.Publish(eventMessage, cancellationToken);

            await repository.DeleteBasket(command.BasketCheckoutDto.UserName);

            return new CheckoutBasketCommandResult(true);

        }
    }
}
