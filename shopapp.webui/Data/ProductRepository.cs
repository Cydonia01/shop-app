using System.Collections.Generic;
using System.Linq;
using shopapp.webui.Models;

namespace shopapp.webui
{
    public class ProductRepository
    {
        private static List<Product> _products = null;
        static ProductRepository()
        {
            _products = new List<Product>
            {
                new Product {ProductId = 1, Name = "Samsung S6", Price = 2000, Description = "Good Phone", ImageUrl = "1.jpg", CategoryId = 1},
                new Product {ProductId = 2, Name = "Samsung S7", Price = 3000, Description = "Good Phone", ImageUrl = "2.jpg", CategoryId = 1},
                new Product {ProductId = 3, Name = "Samsung S8", Price = 4000, Description = "Good Phone", ImageUrl = "3.jpg", CategoryId = 1},
                new Product {ProductId = 4, Name = "Samsung S9", Price = 5000, Description = "Good Phone", ImageUrl = "4.jpg", CategoryId = 1},
                new Product {ProductId = 5, Name = "Samsung S10", Price = 6000, Description = "Good Phone", ImageUrl = "5.jpg", CategoryId = 1},
                new Product {ProductId = 6, Name = "Lenovo S6", Price = 2000, Description = "Good Computer", ImageUrl = "1.jpg", CategoryId = 2},
                new Product {ProductId = 7, Name = "Lenovo S7", Price = 3000, Description = "Good Computer", ImageUrl = "2.jpg", CategoryId = 2},
                new Product {ProductId = 8, Name = "Lenovo S8", Price = 4000, Description = "Good Computer", ImageUrl = "3.jpg", CategoryId = 2},
                new Product {ProductId = 9, Name = "Lenovo S9", Price = 5000, Description = "Good Computer", ImageUrl = "4.jpg", CategoryId = 2},
                new Product {ProductId = 10, Name = "Lenovo S10", Price = 6000, Description = "Good Computer", ImageUrl = "5.jpg", CategoryId = 2},
            };
        }

        public static List<Product> Products
        {
            get
            {
                return _products;
            }
        }

        public static void AddProduct(Product entity)
        {
            _products.Add(entity);
        }

        public static Product GetProductById(int id)
        {
            return _products.FirstOrDefault(i => i.ProductId == id);
        }
    
        public static void EditProduct(Product product) {
            foreach (var p in _products) {
                if (p.ProductId == product.ProductId) {
                    p.Name = product.Name;
                    p.Price = product.Price;
                    p.Description = product.Description;
                    p.ImageUrl = product.ImageUrl;
                    p.CategoryId = product.CategoryId;
                }
            }
        }

        public static void DeleteProduct(int id) {
            var product = GetProductById(id);
            if (product != null) {
                _products.Remove(product);
            }
        }
    }
}