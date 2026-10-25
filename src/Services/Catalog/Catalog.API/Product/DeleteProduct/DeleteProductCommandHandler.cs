
namespace Catalog.API.Product.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsDeleted);

    internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) 
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling DeleteProductCommand for Id: {Id}", query.Id);

            var existingProduct = await session.LoadAsync<Models.Product>(query.Id, cancellationToken);

            if (existingProduct is null)
                throw new ProductNotFoundException();

            session.Delete(existingProduct);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
