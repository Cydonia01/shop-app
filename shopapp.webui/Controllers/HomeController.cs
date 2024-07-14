using Microsoft.AspNetCore.Mvc;

namespace shopapp.webui.Controllers {
    public class HomeController: Controller {
        public IActionResult Index() {
            
            var productViewModel = new ProductViewModel {
                Products = ProductRepository.Products
            };
            return View(productViewModel);
        }

        public IActionResult About() {
            return View();
        }
    }
}