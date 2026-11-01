
namespace Basket.API.Data
{
    // this decorator class to add caching to services without editing in original services
    public class CachedBasketRepository : IBasketRepository
    {
        private readonly IBasketRepository repository;
        private readonly IDistributedCache cache;

        public CachedBasketRepository(IBasketRepository repository, IDistributedCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            var basket = await repository.GetBasket(userName, cancellationToken);

            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await repository.StoreBasket(basket, cancellationToken);
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userName, cancellationToken);

            await cache.RemoveAsync(userName, cancellationToken);

            return true;
        }
    }
}
