using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;

namespace shopapp.webui.Controllers {
    public class HomeController: Controller {
        private IProductService _productService;

        public HomeController(IProductService productService) {
            _productService = productService;
        }
        public IActionResult Index() {
            
            var productViewModel = new ProductListViewModel {
                Products = _productService.GetAll()
            };
            return View(productViewModel);
        }

        public IActionResult About() {
            return View();
        }
        
    }
}