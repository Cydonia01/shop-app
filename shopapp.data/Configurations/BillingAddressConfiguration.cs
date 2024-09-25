/*
This class is used to configure the properties of the BillingAddress class.
*/
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;

namespace shopapp.data.Configurations
{
    public class BillingAddressConfiguration : IEntityTypeConfiguration<BillingAddress>
    {
        public void Configure(EntityTypeBuilder<BillingAddress> builder)
        {
            builder.HasKey(p => p.BillingAddressId);
            builder.Property(p => p.Address).IsRequired();
            builder.Property(p => p.City).IsRequired();
            builder.Property(p => p.Country).IsRequired();
            builder.Property(p => p.ZipCode).IsRequired().HasMaxLength(10);
        }
    }
}