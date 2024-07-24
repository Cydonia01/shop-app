using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category>, ICategoryRepository
    {
        public EfCoreCategoryRepository(ShopContext context): base(context)
        {
            
        }
        private ShopContext ShopContext {
            get { return context as ShopContext; }
        }

        public Category GetByIdWithProducts(int categoryId)
        {
                return ShopContext.Categories
                .Where(i=>i.CategoryId==categoryId)
                .Include(i=>i.ProductCategories)
                .ThenInclude(i=>i.Product)
                .FirstOrDefault();
        }
        public void DeleteFromCategory(int productId, int categoryId)
        {
                var cmd = @"delete from productcategory where ProductId=@p0 and CategoryId=@p1";
                
                ShopContext.Database.ExecuteSqlRaw(cmd, productId, categoryId);
        }
    }
}