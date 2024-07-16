using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    public class AdminController: Controller
    {
        private IProductService _productService;

        public AdminController(IProductService productService) {
            _productService = productService;
        }    
        
        public IActionResult ProductList() {
            return View(new ProductListViewModel() {
                Products = _productService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CreateProduct() {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model) {
            var entity = new Product() {
                Name = model.Name,
                Url = model.Url,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = model.ImageUrl
            };
            _productService.Create(entity);
            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public IActionResult Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var entity = _productService.GetById((int)id);
            if (entity == null) {
                return NotFound();
            }
            
            var model = new ProductModel() {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductModel model) {
            var entity = _productService.GetById(model.ProductId);
            
            if (entity == null) {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Url = model.Url;
            entity.Price = model.Price;
            entity.Description = model.Description;
            entity.ImageUrl = model.ImageUrl;

            _productService.Update(entity);

            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteProduct(int productId) {
            var entity = _productService.GetById(productId);
            if (entity != null) {
                _productService.Delete(entity);
            }
            
            return RedirectToAction("ProductList");
        }
    }
}