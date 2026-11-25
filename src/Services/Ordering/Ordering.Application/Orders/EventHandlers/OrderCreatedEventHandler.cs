using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderCreatedEventHandler(IPublishEndpoint publisher, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain event handled: {DomainEvent}", domainEvent.GetType().Name);

            if (!await featureManager.IsEnabledAsync("OrderFullfilment"))
            {
                var order = domainEvent.order.toOrderDto();

                await publisher.Publish(order, cancellationToken);
            }
        }
    }
}
