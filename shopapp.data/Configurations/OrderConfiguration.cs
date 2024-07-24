using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;

namespace shopapp.data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(p => p.OrderId);

            builder.HasOne(p => p.ShippingAddress)
            .WithMany()
            .HasForeignKey(p => p.ShippingAddressId);

            builder.HasOne(p => p.BillingAddress)
            .WithMany()
            .HasForeignKey(p => p.BillingAddressId);

            builder.HasOne(p => p.Card)
            .WithMany()
            .HasForeignKey(p => p.CardId);
        }
    }
}