using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers {
    public class ProductController : Controller {
        // localhost:5000/product/list
        public IActionResult Index() {
            return View();
        }

        public IActionResult List(int? id, string q) {
            var products = ProductRepository.Products;

            if (id != null) {
                products = products.Where(i => i.CategoryId == id).ToList();
            }

            if (!string.IsNullOrEmpty(q)) {
                products = products.Where(i => i.Name.ToLower().Contains(q.ToLower()) || i.Description.Contains(q)).ToList();
            }

            var productViewModel = new ProductViewModel {
                Products = products
            };
            return View(productViewModel);
        }

        public IActionResult Details(int id) {
            return View(ProductRepository.GetProductById(id));
        }

        [HttpGet]
        public IActionResult Create() {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product p) {
            if (ModelState.IsValid) {
                ProductRepository.AddProduct(p);
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View(p);
        }
        
        [HttpGet]
        public IActionResult Edit(int id) {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View(ProductRepository.GetProductById(id));
        }

        [HttpPost]
        public IActionResult Edit(Product p) {
            ProductRepository.EditProduct(p);
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Delete(int productId) {
            ProductRepository.DeleteProduct(productId);
            return RedirectToAction("List");
        }
    }
}