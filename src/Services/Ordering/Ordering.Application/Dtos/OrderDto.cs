

using Ordering.Domain.Enum;

namespace Ordering.Application.Dtos
{
    public record OrderDto
    (
         Guid Id,
         Guid CustomeId,
         string OrderName,
         AddressDto ShippingAddress,
         AddressDto BillingAddress,
         PaymentDto Payment,
         OrderStatus Status,
         List<OrderItemDto> Items
    );
}
