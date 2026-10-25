
namespace Catalog.API.Product.GetProductByCategory
{
    //public record GetProductsByCategoryRequest(string Category)

    public record GetProductsByCategoryResponse(IEnumerable<Models.Product> Products);


    public class GetProductsByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));

                var response = result.Adapt<GetProductsByCategoryResponse>();

                return Results.Ok(response);
            })
            .WithDescription("Get Products By Category")
            .WithSummary("Get Products By Category")
            .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
