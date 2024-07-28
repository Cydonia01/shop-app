/*
* This class is used to control the shop page.
* It is used to display the products in the shop page.
*/

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    public class ShopController:Controller
    {
        private readonly IProductService _productService;

        public ShopController(IProductService productService) {
            _productService = productService;
        }

        // List the products
        public IActionResult List(string category, int page = 1) {
            const int pageSize = 3;
            // Create a new ProductListViewModel object
            var productViewModel = new ProductListViewModel {
                PageInfo = new PageInfo() {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = _productService.GetProductsByCategory(category,pageSize, page)
            };
            return View(productViewModel);
        }

        // Display the details of a product
        public IActionResult Details(string url) {
            if (url == null) {
                return NotFound();
            }

            // Get the product details using the url
            Product product = _productService.GetProductDetails(url);

            if (product == null) {
                return NotFound();
            }

            return View(new ProductDetailModel() {
                Product = product,
                Categories = product.ProductCategories.Select(i=>i.Category).ToList()
            });
        }

        // Search for a product using a search query by product name and description
        public IActionResult Search(string q) {
            q ??= ""; // If q is null, set it to an empty string
            var productViewModel = new ProductListViewModel {
                Products = _productService.GetSearchResult(q)
            };
            return View(productViewModel);
        }
    }
}