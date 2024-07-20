using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shopapp.entity;

namespace shopapp.data.Configurations
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>().HasData(
                new Product() {ProductId = 1, Name = "Samsung S5", Url="samsung-s5", Price = 2000, ImageUrl = "1.jpg", Description = "Nice Phone", IsApproved = true },
                new Product() {ProductId = 2,  Name = "Samsung S6", Url="samsung-s6", Price = 3000, ImageUrl = "2.jpg", Description = "Nice Phone", IsApproved = true },
                new Product() {ProductId = 3,  Name = "Samsung S7", Url="samsung-s7", Price = 4000, ImageUrl = "3.jpg", Description = "Nice Phone", IsApproved = false },
                new Product() {ProductId = 4,  Name = "Samsung S8", Url="samsung-s8", Price = 5000, ImageUrl = "4.jpg", Description = "Nice Phone", IsApproved = true },
                new Product() {ProductId = 5,  Name = "Samsung S9", Url="samsung-s9", Price = 6000, ImageUrl = "5.jpg", Description = "Nice Phone", IsApproved = true }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category() {CategoryId = 1, Name = "Phone", Url="phone"},
                new Category() {CategoryId = 2, Name = "Computer", Url="computer"},
                new Category() {CategoryId = 3, Name = "Electronics", Url="electronics"},
                new Category() {CategoryId = 4, Name = "Household Appliances", Url="household-appliances"}
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory() {CategoryId = 1, ProductId = 1},
                new ProductCategory() {CategoryId = 1, ProductId = 2},
                new ProductCategory() {CategoryId = 1, ProductId = 3},
                new ProductCategory() {CategoryId = 2, ProductId = 4},
                new ProductCategory() {CategoryId = 2, ProductId = 5},
                new ProductCategory() {CategoryId = 3, ProductId = 1},
                new ProductCategory() {CategoryId = 3, ProductId = 2},
                new ProductCategory() {CategoryId = 4, ProductId = 3}
            );
        }
    }
}