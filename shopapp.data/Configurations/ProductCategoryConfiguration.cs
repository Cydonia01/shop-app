/*
    This class is used to configure the relationship between the Product and Category entities.
*/
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;

namespace shopapp.data.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(c => new {c.CategoryId, c.ProductId});
            builder.HasOne(c => c.Product).WithMany(c => c.ProductCategories).HasForeignKey(c => c.ProductId);
            builder.HasOne(c => c.Category).WithMany(c => c.ProductCategories).HasForeignKey(c => c.CategoryId);
        }
    }
}