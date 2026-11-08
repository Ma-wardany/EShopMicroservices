using Ordering.Domain.Excptions;

namespace Ordering.Domain.ValueObjects
{
    public record OrderName
    {
        public string Value { get; }

        private OrderName(string value) => Value = value;

        public static OrderName Of(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new DomainException("order name cannot be empty");

            return new OrderName(value);
        }
    }
}
