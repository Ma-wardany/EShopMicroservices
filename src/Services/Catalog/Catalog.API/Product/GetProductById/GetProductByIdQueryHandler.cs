namespace Catalog.API.Product.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

    public record GetProductByIdResult(Models.Product Product);

    internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductByIdQuery for Id: {Id}", query.Id);

            var product = await session.LoadAsync<Models.Product>(query.Id, cancellationToken);

            if (product is null)
                throw new ProductNotFoundException();

            return new GetProductByIdResult(product);
        }
    }
}
