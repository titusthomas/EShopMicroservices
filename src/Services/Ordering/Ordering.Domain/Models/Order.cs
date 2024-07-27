

namespace Ordering.Domain.Models
{
    public class Order:Aggregate<OrderId>
    {
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> orderItems=> _orderItems.AsReadOnly();
        public CustomerId CustomeId { private set; get; } = default!;
        public OrderName OrderName { private set; get; } = default!;
        public Address ShippingAddress { private set; get; } = default!;
        public Address BillingAddress { private set; get; } = default!;
        public Payment Payment { private set; get; } = default!;
        public OrderStatus Status { private set; get; } = OrderStatus.Pending;
        public decimal Total
        {
            get=>orderItems.Sum(x => x.Price * x.Quantity);
            private set { }
        }

        public static Order Create(OrderId id, CustomerId customerId,OrderName orderName,Address shippingAddress, Address billingAddress,Payment payment)
        {
            var order = new Order()
            {
                Id = id,
                Payment = payment,
                BillingAddress = billingAddress,
                ShippingAddress = shippingAddress,
                Status = OrderStatus.Pending,
                CustomeId = customerId,
                OrderName = orderName
            };
            order.AddDomainEvent(new OrderCreatedEvent(order));
            return order;
        }

        public void Update( OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment,OrderStatus status)
        {
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            Status = status;
            AddDomainEvent(new OrderUpdatedEvent(this));
        }
        public void Add(ProductId productId, int qnantity, decimal price) 
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(qnantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var orderitem=new OrderItem(Id, productId, qnantity, price);

            _orderItems.Add(orderitem);
        }
        public void Remove(ProductId productId) 
        {
            var orderItem=_orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (orderItem is not null)
            {
                _orderItems.Remove(orderItem);
            }
        }
    }
}
