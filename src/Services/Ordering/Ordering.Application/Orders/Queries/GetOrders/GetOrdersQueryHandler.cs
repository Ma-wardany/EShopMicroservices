
using BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersQueryHandler(IApplicationDbContext context) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = await context.Orders.LongCountAsync(cancellationToken);

            var orders = await context.Orders
                                      .Include(o => o.OrderItems)
                                      .AsNoTracking()
                                      .Skip(pageSize * (pageIndex-1))
                                      .Take(pageSize)
                                      .ToListAsync();

            var orderDtos = orders.ToOrderDtoList();

            return new GetOrdersResult(new PaginationResult<OrderDto>(
                pageIndex,
                pageSize,
                totalCount,
                orderDtos));
        }
    }
}
