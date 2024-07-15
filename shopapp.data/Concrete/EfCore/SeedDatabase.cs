using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Concrete.EfCore;
using shopapp.entity;

namespace shopapp.data.Concrete
{
    public class SeedDatabase
    {
        public static Product[] Products = {
            new Product() { Name = "Samsung S5", Price = 2000, ImageUrl = "1.jpg", Description = "Nice Phone", IsApproved = true },
            new Product() { Name = "Samsung S6", Price = 3000, ImageUrl = "2.jpg", Description = "Nice Phone", IsApproved = true },
            new Product() { Name = "Samsung S7", Price = 4000, ImageUrl = "3.jpg", Description = "Nice Phone", IsApproved = false },
            new Product() { Name = "Samsung S8", Price = 5000, ImageUrl = "4.jpg", Description = "Nice Phone", IsApproved = true },
            new Product() { Name = "Samsung S9", Price = 6000, ImageUrl = "5.jpg", Description = "Nice Phone", IsApproved = true }
        };
        
        public static Category[] Categories = {
            new Category() { Name = "Phone", Url="phone"},
            new Category() { Name = "Computer", Url="computer"},
            new Category() { Name = "Electronics", Url="electronics"},
            new Category() { Name = "Household Appliances", Url="household-appliances"}
        };

        public static void Seed() {
            var context = new ShopContext();
            if (context.Database.GetPendingMigrations().Count() == 0) {
                if(context.Categories.Count() == 0) {
                    context.Categories.AddRange(Categories);
                }
                if(context.Products.Count() == 0) {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
            }
            context.SaveChanges();
        }

        private static ProductCategory[] ProductCategories =
        {
            new ProductCategory() {
                Product=Products[0], Category=Categories[0]
            },
            new ProductCategory() {
                Product=Products[0], Category=Categories[2]
            },
            new ProductCategory() {
                Product=Products[1], Category=Categories[0]
            },
            new ProductCategory() {
                Product=Products[1], Category=Categories[2]
            },
            new ProductCategory() {
                Product=Products[2], Category=Categories[0]
            },
            new ProductCategory() {
                Product=Products[2], Category=Categories[2]
            },
            new ProductCategory() {
                Product=Products[3], Category=Categories[0]
            },
            new ProductCategory() {
                Product=Products[3], Category=Categories[2]
            },
        };
        
    }
}