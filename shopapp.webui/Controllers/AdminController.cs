using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Extensions;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [Authorize]
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
            if (ModelState.IsValid) {
                var entity = new Product() {
                    Name = model.Name,
                    Url = model.Url,
                    Price = model.Price,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };

                if(_productService.Create(entity)) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Product Created.",
                        Message = $"{entity.Name} was created successfully.",
                        AlertType = "success"
                    });                    
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage() {
                    Title = "Product not Created.",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });     
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductEdit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);
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
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i=>i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds, IFormFile file) {
            if(ModelState.IsValid) {
                var entity = _productService.GetById(model.ProductId);
                
                if (entity == null) {
                    return NotFound();
                }

                entity.Name = model.Name;
                entity.Url = model.Url;
                entity.Price = model.Price;
                entity.Description = model.Description;
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;

                if(file != null) {
                    var extension = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);
                    
                    using(var stream = new FileStream(path, FileMode.Create)) {
                        await file.CopyToAsync(stream);
                    }
                }

                if(_productService.Update(entity, categoryIds)) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Product Updated.",
                        Message = $"{entity.Name} was updated successfully.",
                        AlertType = "success"
                    });
                    
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage() {
                    Title = "Product not Updated.",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        public IActionResult DeleteProduct(int productId) {
            var entity = _productService.GetById(productId);
            if (entity != null) {
                _productService.Delete(entity);
            }
            TempData.Put("message", new AlertMessage() {
                Title = "Product Deleted.",
                Message = $"{entity.Name} was deleted successfully.",
                AlertType = "success"
            });
            
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
            if(ModelState.IsValid) {
                var entity = new Category() {
                    Name = model.Name,
                    Url = model.Url,
                };

                if (_categoryService.Create(entity)) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Category Created.",
                        Message = $"{entity.Name} category was created successfully.",
                        AlertType = "success"
                    });
                    return RedirectToAction("CategoryList");
                }
                TempData.Put("message", new AlertMessage() {
                    Title = "Category not Created.",
                    Message = _categoryService.ErrorMessage,
                    AlertType = "danger"
                });
            }
            return View(model);
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
            if(ModelState.IsValid) {
                var entity = _categoryService.GetById(model.CategoryId);
                
                if (entity == null) {
                    return NotFound();
                }

                entity.Name = model.Name;
                entity.Url = model.Url;

                if(_categoryService.Update(entity)) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Category Updated.",
                        Message = $"{entity.Name} category was updated successfully.",
                        AlertType = "success"
                    });
                    return RedirectToAction("CategoryList");
                }
                TempData.Put("message", new AlertMessage() {
                    Title = "Category not Updated.",
                    Message = _categoryService.ErrorMessage,
                    AlertType = "danger"
                });
            }
            return View(model);
        }

        public IActionResult DeleteCategory(int CategoryId) {
            var entity = _categoryService.GetById(CategoryId);
            if (entity != null) {
                _categoryService.Delete(entity);
            }

            TempData.Put("message", new AlertMessage() {
                Title = "Category Deleted.",
                Message = $"{entity.Name} category was deleted successfully.",
                AlertType = "success"
            });

            return RedirectToAction("CategoryList");
        }
    
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId) {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/" + categoryId);
        }
    }
}