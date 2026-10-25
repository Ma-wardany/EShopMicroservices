
namespace Catalog.API.Product.GetProductById
{
    //public record GetProductByIdRequest(Guid Id);

    public record GetProductByIdResponse(Models.Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (Guid Id, ISender sender) =>
            {
                try
                {
                    var result = await sender.Send(new GetProductByIdQuery(Id));

                    var response = result.Adapt<GetProductByIdResponse>();

                    return Results.Ok(response);
                }
                catch (ProductNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }

            })
            .WithDescription("Get Product By Id")
            .WithSummary("Get Product By Id")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
