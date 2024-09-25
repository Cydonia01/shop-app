/*
This class is used to configure the Category entity. It is used to define the properties of the Category entity and the relationships between the entities.
*/
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;

namespace shopapp.data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(p => p.CategoryId);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Url).IsRequired().HasMaxLength(100);
            builder.HasMany(p => p.ProductCategories).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId);
            
        }
    }
}