
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers {
    public class HomeController: Controller {
        public IActionResult Index() {
            
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

        public IActionResult About() {
            return View();
        }

        public IActionResult Contact() {
            return View("MyView");
        }
    }
}