

namespace Ordering.Domain.ValueObjects
{
    public record OrderItemId
    {

        public Guid Value { get; }
        private OrderItemId(Guid guid)=>Value = guid;
        public static OrderItemId Of(Guid value)
            {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("Order Item id cannot be empty");
            }
            return new OrderItemId(value); 
        }
    }
}
