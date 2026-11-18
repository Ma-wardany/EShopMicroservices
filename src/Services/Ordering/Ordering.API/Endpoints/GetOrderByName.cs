using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.DeleteOrder;
using Ordering.Application.Orders.Queries.GetOrderByName;
using Ordering.Domain.ValueObjects;

namespace Ordering.API.Endpoints
{
    public record GetOrderByNameRequest(string Name);

    public record GetOrderByNameResponse(IEnumerable<OrderDto> Orders);

    public class GetOrderByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
            {
                var result = await sender.Send(new GetOrderByNameQuery(orderName));

                var response = result.Adapt<GetOrderByNameResponse>();

                return Results.Ok(response);
            });
        }
    }
}
