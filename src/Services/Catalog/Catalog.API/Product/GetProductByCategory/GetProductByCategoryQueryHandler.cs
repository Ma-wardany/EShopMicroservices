
namespace Catalog.API.Product.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<Models.Product> Products);

    internal class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger) 
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductByCategoryQuery for Category: {Category}", query.Category);

            var products = await session.Query<Models.Product>()
                .Where(p => p.Category.Contains(query.Category)).ToListAsync(cancellationToken);

            return new GetProductByCategoryResult(products);
        }
    }
}
