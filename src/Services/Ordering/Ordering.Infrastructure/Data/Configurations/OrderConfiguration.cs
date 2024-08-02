

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enum;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System.Security.Cryptography.X509Certificates;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasConversion(o => o.Value, dbid => OrderId.Of(dbid));
            //  builder.Property(o => o.Name).HasMaxLength(100).IsRequired();
            builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomeId).IsRequired();
            builder.HasMany(o=>o.orderItems).WithOne().HasForeignKey(o => o.OrderId);

            builder.ComplexProperty(
                o => o.OrderName, namebuilder =>
                {
                    namebuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
                    
                }
            );
            builder.ComplexProperty(o => o.ShippingAddress,
                addressbuilder =>
                {
                    addressbuilder.Property(a=>a.FirstName).HasMaxLength(50).IsRequired();
                    addressbuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
                    addressbuilder.Property(a => a.EmailAddress).HasMaxLength(50);
                    addressbuilder.Property(a => a.AddressLine).HasMaxLength(150).IsRequired();
                    addressbuilder.Property(a => a.Country).HasMaxLength(50);
                    addressbuilder.Property(a => a.State).HasMaxLength(50);
                    addressbuilder.Property(a => a.Zipcode).HasMaxLength(5).IsRequired();
                });
            builder.ComplexProperty(o => o.BillingAddress,
                addressbuilder =>
                {
                    addressbuilder.Property(a=>a.FirstName).HasMaxLength(50).IsRequired();
                    addressbuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
                    addressbuilder.Property(a => a.EmailAddress).HasMaxLength(50);
                    addressbuilder.Property(a => a.AddressLine).HasMaxLength(150).IsRequired();
                    addressbuilder.Property(a => a.Country).HasMaxLength(50);
                    addressbuilder.Property(a => a.State).HasMaxLength(50);
                    addressbuilder.Property(a => a.Zipcode).HasMaxLength(5).IsRequired();
                });
            builder.ComplexProperty(o => o.Payment,
                paymentbuilder =>
                {
                    paymentbuilder.Property(p => p.CardNumber).HasMaxLength(24).IsRequired();
                    paymentbuilder.Property(p => p.CardName).HasMaxLength(50).IsRequired();
                    paymentbuilder.Property(p => p.CVV).HasMaxLength(3);
                    paymentbuilder.Property(p => p.Expiration).HasMaxLength(10);
                    paymentbuilder.Property(p => p.PaymentMethod);
                });
            builder.Property(o=>o.Status).HasDefaultValue(OrderStatus.Draft)
                .HasConversion(s=>s.ToString(),dbid=>(OrderStatus)Enum.Parse(typeof(OrderStatus),dbid));
            builder.Property(o => o.Total);
        }
    }
}
