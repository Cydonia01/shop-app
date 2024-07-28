/*
* This class is used to define the ShopContext class.
* It is used to connect to the database.
*/
using Microsoft.EntityFrameworkCore;
using shopapp.data.Configurations;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class ShopContext: DbContext
    {
        // Constructor: Initializes the ShopContext class.
        public ShopContext(DbContextOptions options): base(options) {}

        // DbSet is used to define the entities.
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<BillingAddress> BillingAdresses { get; set; }
        public DbSet<ShippingAddress> ShippingAdresses { get; set; }

        // If you want to use SQLite, you can use the following code:
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlite("Data Source=shopDb");
        // }

        // This method is used to configure the entities.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new CardConfiguration());
            modelBuilder.ApplyConfiguration(new BillingAddressConfiguration());
            modelBuilder.ApplyConfiguration(new ShippingAddressConfiguration());

            modelBuilder.Seed();
        }

    }
}