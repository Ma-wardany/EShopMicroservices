
namespace Catalog.API.Product.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) 
        : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) 
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling UpdateProductCommand for Id: {Id}", command.Id);

            var existingProduct = await session.LoadAsync<Models.Product>(command.Id);

            if (existingProduct is null)
                throw new ProductNotFoundException();

            existingProduct.Name = command.Name;
            existingProduct.Category = command.Category;
            existingProduct.Description = command.Description;
            existingProduct.ImageFile = command.ImageFile;
            existingProduct.Price = command.Price;


            session.Update(existingProduct);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
