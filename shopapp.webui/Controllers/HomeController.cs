using Microsoft.AspNetCore.Mvc;
using shopapp.data.Abstract;

namespace shopapp.webui.Controllers {
    public class HomeController: Controller {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }
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