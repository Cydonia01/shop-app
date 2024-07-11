using Microsoft.AspNetCore.Mvc;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers {
    public class ProductController : Controller {
        // localhost:5000/product/list
        public IActionResult Index() {

            // ViewBag
            // Model
            // ViewData

            var product = new Product() {
                Name = "Iphone X",
                Price = 6000,
                Description = "Nice phone!"
            };

            // ViewData["Product"] = product;
            // ViewData["Category"] = "Phones";

            // ViewBag.Product = product;
            ViewBag.Category = "Phones";

            return View(product);
        }

        public IActionResult List() {
            return View();
        }

        public IActionResult Details(int id) {
            // ViewBag.Name = "Samsung S6";
            // ViewBag.Price = 3000;
            // ViewBag.Description = "Nice phone!";
            
            var p = new Product() {
                Name = "Samsung S6",
                Price = 3000,
                Description = "Nice phone!"
            };
            
            return View(p);
        }
    }
}