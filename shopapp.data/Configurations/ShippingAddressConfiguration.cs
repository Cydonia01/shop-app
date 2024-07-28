using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;

namespace shopapp.data.Configurations
{
    public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
    {
        public void Configure(EntityTypeBuilder<ShippingAddress> builder)
        {
            builder.HasKey(p => p.ShippingAddressId);
            builder.Property(p => p.Address).IsRequired();
            builder.Property(p => p.City).IsRequired();
            builder.Property(p => p.Country).IsRequired();
            builder.Property(p => p.ZipCode).IsRequired().HasMaxLength(10);
        }
    }
}