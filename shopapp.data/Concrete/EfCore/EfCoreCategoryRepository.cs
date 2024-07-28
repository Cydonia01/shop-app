/*
* This class implements the ICategoryRepository interface.
* It is used to implement the methods of the CategoryRepository class.
*/
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category>, ICategoryRepository
    {
        public EfCoreCategoryRepository(ShopContext context): base(context) {}
        
        // Define the ShopContext property
        private ShopContext ShopContext {
            get { return context as ShopContext; }
        }

        // Gets the category by the id with the products
        public Category GetByIdWithProducts(int categoryId)
        {
                return ShopContext.Categories
                .Where(i=>i.CategoryId==categoryId)
                .Include(i=>i.ProductCategories) // Same as joining the ProductCategories table
                .ThenInclude(i=>i.Product) // Same as joining the Product table
                .FirstOrDefault();
        }
        
        // Deletes a product from the category
        public void DeleteFromCategory(int productId, int categoryId)
        {
                var cmd = @"delete from productcategory where ProductId=@p0 and CategoryId=@p1";
                ShopContext.Database.ExecuteSqlRaw(cmd, productId, categoryId);
        }
    }
}