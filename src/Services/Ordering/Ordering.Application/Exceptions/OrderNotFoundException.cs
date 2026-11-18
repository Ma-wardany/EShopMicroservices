using BuildingBlocks.Excptions;

namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid id) : base("Order", id)
        {
        }
    }
}
