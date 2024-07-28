using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;

namespace shopapp.data.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(p => p.CardId);
            builder.Property(p => p.CardName).IsRequired();
            builder.Property(p => p.CardNumber).IsRequired().HasMaxLength(16);
            builder.Property(p => p.Cvc).IsRequired().HasMaxLength(3);
            builder.Property(p => p.ExpirationMonth).IsRequired().HasMaxLength(2);
            builder.Property(p => p.ExpirationYear).IsRequired().HasMaxLength(4);

        }
    }
}