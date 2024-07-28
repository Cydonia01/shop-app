/*
* EfCoreProductRepository class is created for the implementation of IProductRepository interface.
*/

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product>, IProductRepository
    {
        public EfCoreProductRepository(ShopContext context): base(context) {}

        // Define the ShopContext property
        private ShopContext ShopContext {
            get { return context as ShopContext; }
        }
        
        // Gets the product by the url
        public Product GetProductDetails(string url)
        {
            return ShopContext.Products
                .Where(i => i.Url == url)
                .Include(i=>i.ProductCategories)
                .ThenInclude(i=>i.Category)
                .FirstOrDefault();
        }

        // Gets the products by the category
        public List<Product> GetProductsByCategory(string name, int pageSize, int page)
        {
            var products = ShopContext.Products
            .Where(i=>i.IsApproved)
            .AsQueryable();

            if (!string.IsNullOrEmpty(name)) {
                products = products
                    .Include(i=>i.ProductCategories)
                    .ThenInclude(i=>i.Category)
                    .Where(i=>i.ProductCategories.Any(a=>a.Category.Url == name));
            }

            // Skip() is used to skip the products on the previous pages
            return products
            .Skip((page-1) * pageSize)
            .Take(pageSize)
            .ToList();
        }

        // Gets the count of the products by the category
        public int GetCountByCategory(string category)
        {
            var products = ShopContext.Products
            .Where(i=>i.IsApproved)
            .AsQueryable();
            if (!string.IsNullOrEmpty(category)) {
                products = products
                    .Include(i=>i.ProductCategories)
                    .ThenInclude(i=>i.Category)
                    .Where(i=>i.ProductCategories.Any(a=>a.Category.Url == category));
            }
            return products.Count();
        }

        // Gets the home page products
        public List<Product> GetHomePageProducts()
        {
            return ShopContext.Products
                .Where(i=>i.IsApproved && i.IsHome)
                .ToList();
        }

        // Gets the search result. It searches the product by the name and description
        public List<Product> GetSearchResult(string searchString)
        {
            var products = ShopContext.Products
                .Where(i=>i.IsApproved && (i.Name.ToLower().Contains(searchString.ToLower()) || i.Description.ToLower().Contains(searchString.ToLower())))
                .AsQueryable();
            return products.ToList();
        }

        // Gets the product by the id with the categories
        public Product GetByIdWithCategories(int id)
        {
            return ShopContext.Products
                .Where(i => i.ProductId == id)
                .Include(i => i.ProductCategories)
                .ThenInclude(i => i.Category)
                .FirstOrDefault();
        }

        // Updates the product
        public void Update(Product entity, int[] categoryIds)
        {
            // Get the product by the id
            var product = ShopContext.Products
                .Include(i => i.ProductCategories)
                .FirstOrDefault(i => i.ProductId == entity.ProductId);

            // If the product is not null, update the product
            if (product != null) {
                product.Name = entity.Name;
                product.Url = entity.Url;
                product.Price = entity.Price;
                product.Description = entity.Description;
                product.ImageUrl = entity.ImageUrl;
                product.IsApproved = entity.IsApproved;
                product.IsHome = entity.IsHome;

                product.ProductCategories = categoryIds.Select(catid => new ProductCategory() {
                    CategoryId = catid,
                    ProductId = entity.ProductId
                }).ToList();
            }

        }
    }
}