using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers {
    public class HomeController: Controller {
        private IProductService _productService;

        public HomeController(IProductService productService) {
            _productService = productService;
        }
        public IActionResult Index() {
            
            var productViewModel = new ProductListViewModel {
                Products = _productService.GetHomePageProducts()
            };
            return View(productViewModel);
        }

        public IActionResult About() {
            return View();
        }
        
    }
}