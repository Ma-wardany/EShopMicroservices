
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrderByName
{
    public class GetOrderByNameQueryHandler(IApplicationDbContext context) : IQueryHandler<GetOrderByNameQuery, GetOrderByNameResult>
    {
        public async Task<GetOrderByNameResult> Handle(GetOrderByNameQuery query, CancellationToken cancellationToken)
        {
            var orders = await context.Orders
                                      .Include(o => o.OrderItems)
                                      .AsNoTracking()
                                      .Where(o => o.OrderName.Value.Contains(query.Name))
                                      .OrderBy(o => o.OrderName)
                                      .ToArrayAsync(cancellationToken);

            var orderDtos = orders.ToOrderDtoList();
            return new GetOrderByNameResult(orderDtos);
        }
    }
}
