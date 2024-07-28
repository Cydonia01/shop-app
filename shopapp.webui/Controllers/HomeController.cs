/*
* Home controller for the home page
*/
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers {
    public class HomeController: Controller {
        private readonly IProductService _productService;

        public HomeController(IProductService productService) {
            _productService = productService;
        }
        public IActionResult Index() {
            var productViewModel = new ProductListViewModel {
                Products = _productService.GetHomePageProducts()
            };
            return View(productViewModel);
        }
    }
}