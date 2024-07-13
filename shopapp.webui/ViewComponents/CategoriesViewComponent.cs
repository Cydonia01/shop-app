using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace shopapp.webui.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var categories = new List<Category> {
                new Category {
                    Name = "Phones",
                    Description = "Phone category"
                },
                new Category {
                    Name = "Computers",
                    Description = "Computer category"
                },
                new Category {
                    Name = "Electronics",
                    Description = "Electronics category"
                }
            };

            return View(categories);
        }
    }
}