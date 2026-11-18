using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints
{
    //public record DeleteOrderRequest(Guid OrderId);

    public record DeleteOrderResponse(bool IsSuccess);

    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteOrderCommand(id));

                var response = result.Adapt<DeleteOrderResponse>();

                return Results.Ok(response);
            });
        }
    }
}
