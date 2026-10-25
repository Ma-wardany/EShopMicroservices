
namespace Catalog.API.Product.DeleteProduct
{
    //public record DeleteProductEndpointRequest(Guid Id);

    public record DeleteProductEndpointResponse(bool IsDeleted);

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id:guid}", async (Guid Id, ISender sender) =>
            {
                try
                {
                    var command = new DeleteProductCommand(Id);

                    var result = await sender.Send(command);

                    var response = result.Adapt<DeleteProductEndpointResponse>();

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }

            })
            .WithDescription("Delete Product")
            .WithSummary("Delete Product")
            .Produces<DeleteProductEndpointResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
