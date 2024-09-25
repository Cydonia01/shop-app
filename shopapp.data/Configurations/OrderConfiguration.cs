/*
    This class is used to configure the Order entity in the database.
    It is used to define the properties of the Order entity and the relationships between other entities.
*/
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
            builder.Property(p => p.OrderNumber).IsRequired();
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.FirstName).IsRequired();
            builder.Property(p => p.LastName).IsRequired();
            builder.Property(p => p.Phone).IsRequired().HasMaxLength(15);
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.Note).HasMaxLength(200);
            builder.Property(p => p.OrderDate).IsRequired();
            builder.Property(p => p.PaymentId).IsRequired();
            builder.Property(p => p.ConversationId).IsRequired();
            builder.Property(p => p.OrderState).IsRequired();
            builder.Property(p => p.PaymentType).IsRequired();


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