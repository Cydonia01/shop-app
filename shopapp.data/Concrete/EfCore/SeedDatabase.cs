using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Concrete.EfCore;
using shopapp.entity;

namespace shopapp.data.Concrete
{
    public class SeedDatabase
    {

        public static Product[] Products = {
            
        };
        
        public static Category[] Categories = {
            
        };

        public static void Seed() {
            // var context = new ShopContext();
            // if (context.Database.GetPendingMigrations().Count() == 0) {
            //     if(context.Categories.Count() == 0) {
            //         context.Categories.AddRange(Categories);
            //     }
            //     if(context.Products.Count() == 0) {
            //         context.Products.AddRange(Products);
            //         context.AddRange(ProductCategories);
            //     }
            // }
            // context.SaveChanges();
        }

        private static ProductCategory[] ProductCategories =
        {
        };
        
    }
}