using System.Collections.Generic;
using System.Linq;

namespace shopapp.webui.Models
{
    public class CategoryRepository
    {
        public static List<Category> _categories = null;
        
        static CategoryRepository() {
            _categories = new List<Category> {
                new Category {
                    CategoryId = 1,
                    Name = "Phones",
                    Description = "Phone category"
                },
                new Category {
                    CategoryId = 2,
                    Name = "Computers",
                    Description = "Computer category"
                },
                new Category {
                    CategoryId = 3,
                    Name = "Electronics",
                    Description = "Electronics category"
                },
                new Category {
                    CategoryId = 4,
                    Name = "Book",
                    Description = "Book category"
                }
            };
        }

        public static List<Category> Categories {
            get {
                return _categories;
            }
        }

        public static void AddCategory(Category entity) {
            _categories.Add(entity);
        }

        public static Category GetCategoryById(int id) {
            return _categories.FirstOrDefault(i => i.CategoryId == id);
        }
    }
}