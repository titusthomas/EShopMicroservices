
namespace Ordering.Domain.ValueObjects
{
    public record CustomerId
    {
        public Guid Value { get;}
        private CustomerId(Guid guid)=> Value = guid;

        public static CustomerId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("Customer Id cannot be null");
            }

            return new CustomerId(value);
        }
    }
}
