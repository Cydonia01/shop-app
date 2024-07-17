using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    public class AdminController: Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        public AdminController(IProductService productService, ICategoryService categoryService) {
            _productService = productService;
            _categoryService = categoryService;
        }    
        
        public IActionResult ProductList() {
            return View(new ProductListViewModel() {
                Products = _productService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult ProductCreate() {
            return View();
        }

        [HttpPost]
        public IActionResult ProductCreate(ProductModel model) {
            var entity = new Product() {
                Name = model.Name,
                Url = model.Url,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = model.ImageUrl
            };
            _productService.Create(entity);

            var msg = new AlertMessage() {
                Message = $"{entity.Name} was created successfully.",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public IActionResult ProductEdit(int? id) {
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
        public IActionResult ProductEdit(ProductModel model) {
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

            var msg = new AlertMessage() {
                Message = $"{entity.Name} was updated successfully.",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteProduct(int productId) {
            var entity = _productService.GetById(productId);
            if (entity != null) {
                _productService.Delete(entity);
            }

            var msg = new AlertMessage() {
                Message = $"{entity.Name} was deleted successfully.",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);
            
            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList() {
            return View(new CategoryListViewModel() {
                Categories = _categoryService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CategoryCreate() {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model) {
            var entity = new Category() {
                Name = model.Name,
                Url = model.Url,
            };
            _categoryService.Create(entity);

            var msg = new AlertMessage() {
                Message = $"{entity.Name} category was created successfully.",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult CategoryEdit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);
            
            if (entity == null) {
                return NotFound();
            }
            
            var model = new CategoryModel() {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories
                    .Select(i => i.Product)
                    .ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model) {
            var entity = _categoryService.GetById(model.CategoryId);
            
            if (entity == null) {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Url = model.Url;

            _categoryService.Update(entity);

            var msg = new AlertMessage() {
                Message = $"{entity.Name} category was updated successfully.",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteCategory(int CategoryId) {
            var entity = _categoryService.GetById(CategoryId);
            if (entity != null) {
                _categoryService.Delete(entity);
            }

            var msg = new AlertMessage() {
                Message = $"{entity.Name} category was deleted successfully.",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);
            
            return RedirectToAction("CategoryList");
        }
    
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId) {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/" + categoryId);
        }
    }
}