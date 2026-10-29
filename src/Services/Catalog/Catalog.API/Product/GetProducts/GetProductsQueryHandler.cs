
using Marten.Pagination;

namespace Catalog.API.Product.GetProducts
{

    public record GetProductsQuery(int? pageNumber = 1, int? pageSize = 10) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Models.Product> Products);

    internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Models.Product>()
                .ToPagedListAsync(query.pageNumber ?? 1, query.pageSize ?? 10, cancellationToken);

            return new GetProductsResult(products);
        }
    }
}
