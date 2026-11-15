
namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler(IApplicationDbContext context) 
        : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {

        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.OrderId);
            var existingOrder = await context.Orders.FindAsync([orderId], cancellationToken);
            if (existingOrder is null)
                throw new OrderNotFoundException(orderId.Value);

            context.Orders.Remove(existingOrder);
            await context.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult(true);
        }
    }
}
