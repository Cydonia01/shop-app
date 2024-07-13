using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Connections;
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
            var products = new List<Product> {
                new Product() {
                    Name = "Samsung S6",
                    Price = 3000,
                    Description = "Nice phone!",
                    IsApproved = true
                },
                new Product() {
                    Name = "Samsung S7",
                    Price = 4000,
                    Description = "Nice phone!",
                    IsApproved = true
                },
                new Product() {
                    Name = "Samsung S8",
                    Price = 5000,
                    Description = "Nice phone!"
                }
            };

            var productViewModel = new ProductViewModel {
                Products = products,
            };
            return View(productViewModel);
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