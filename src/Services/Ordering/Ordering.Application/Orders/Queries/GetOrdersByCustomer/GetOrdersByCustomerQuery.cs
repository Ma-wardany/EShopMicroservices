namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public record class GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerResult>;

    public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);
}
