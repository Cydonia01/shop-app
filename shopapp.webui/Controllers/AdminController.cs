/*
* AdminController.cs file defines the AdminController class.
* The AdminController class is responsible for handling the admin operations such as creating, updating, and deleting products and categories.
* It is also responsible for handling the user and role operations such as creating, updating, and deleting users and roles.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    // Only users with admin role can access this controller
    [Authorize(Roles = "admin")]
    public class AdminController: Controller
    {
        // Dependency Injection
        // Defines the managers and services that will be used in the controller

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        // Constructor: Initializes the AccountController class with the UserManager, SignInManager, EmailSender, and CartService parameters.
        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager) {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> UserEdit(string id) {
            // Find the user by id
            var user = await _userManager.FindByIdAsync(id);
            // If the user is found, return the user details view
            if (user != null) {
                var SelectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);
                ViewBag.Roles = roles;
                return View(new UserDetailsModel() {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = SelectedRoles
                });
            }
            return Redirect("~/admin/user/list");
        }

        // UserEdit method is called when the user details are updated
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles) {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null) {
                    // Update the user details
                    user.UserName = model.UserName;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    // Update the user details in the database
                    var result = await _userManager.UpdateAsync(user);
                    // If the user details are updated successfully, add the user to the selected roles and remove the user from the unselected roles
                    if (result.Succeeded) {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles ??= new string[]{}; // If selectedRoles is null, set it to an empty array
                        
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());

                        TempData.Put("message", new AlertMessage() {
                            Title = "User Updated.",
                            Message = $"{user.UserName} was updated successfully.",
                            AlertType = "success"
                        });

                        return Redirect("~/admin/user/list");
                    }
                }
                // If the user details are not updated successfully, display an error message
                TempData.Put("message", new AlertMessage() {
                    Title = "User not Updated.",
                    Message = "User could not be updated.",
                    AlertType = "danger"
                });
                return Redirect("~/admin/user/list");
            }
            return View(model);
        }

        public IActionResult UserList() {
            return View(_userManager.Users);
        }

        [HttpPost]
        public async Task<IActionResult> UserDelete(string userId) {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null) {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "User Deleted.",
                        Message = $"{user.UserName} was deleted successfully.",
                        AlertType = "success"
                    });
                    return RedirectToAction("UserList");
                }
                foreach (var error in result.Errors) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "User cannot Deleted.",
                        Message = error.Description,
                        AlertType = "danger"
                    });
                }
            }
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> RoleEdit(string id) {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            // If the role is found, add the users to the members and nonmembers lists
            foreach (var user in _userManager.Users.ToList()) {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
                list.Add(user);
            }

            // Create a new RoleDetails object with the role, members, and nonmembers lists
            var model = new RoleDetails() {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model) {
            // For each user in the IdsToAdd list, add the user to the role
            foreach (var userId in model.IdsToAdd ?? new string[]{}) {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null) {
                    var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                    if(!result.Succeeded) {
                        foreach (var error in result.Errors) {
                            TempData.Put("message", new AlertMessage() {
                                Title = "Role cannot Updated.",
                                Message = error.Description,
                                AlertType = "danger"
                            });
                        }
                    } else {
                        TempData.Put("message", new AlertMessage() {
                            Title = "Role Updated.",
                            Message = "Role updated successfully.",
                            AlertType = "success"
                        });
                    }
                }
            }

            // For each user in the IdsToDelete list, remove the user from the role
            foreach (var userId in model.IdsToDelete ?? new string[]{}) {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null) {
                    var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                    if(!result.Succeeded) {
                        foreach (var error in result.Errors) {
                            TempData.Put("message", new AlertMessage() {
                                Title = "Role cannot Updated.",
                                Message = error.Description,
                                AlertType = "danger"
                            });
                        }
                    } else {
                        TempData.Put("message", new AlertMessage() {
                            Title = "Role Updated.",
                            Message = "Role updated successfully.",
                            AlertType = "success"
                        });
                    }
                }
            }
            
            return Redirect("/admin/role/" + model.RoleId);
        }

        public IActionResult RoleList() {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model) {
            if(ModelState.IsValid) {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                // If the role is created successfully, display a success message
                if(result.Succeeded) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Role Created.",
                        Message = $"{model.Name} role created successfully.",
                        AlertType = "success"
                    });
                    return RedirectToAction("RoleList");
                } else {
                    foreach (var error in result.Errors) {
                        TempData.Put("message", new AlertMessage() {
                            Title = "Role not Created.",
                            Message = error.Description,
                            AlertType = "danger"
                        });   
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleDelete(string roleId) {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role != null) {
                var result = await _roleManager.DeleteAsync(role);
                // If the role is deleted successfully, display a success message
                if(result.Succeeded) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Role Deleted.",
                        Message = $"{role.Name} role deleted successfully.",
                        AlertType = "success"
                    });
                    return RedirectToAction("RoleList");
                } else {
                    foreach (var error in result.Errors) {
                        TempData.Put("message", new AlertMessage() {
                            Title = "Role cannot Deleted.",
                            Message = error.Description,
                            AlertType = "danger"
                        });
                    }
                }
            }
            return RedirectToAction("RoleList");
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
                // Create a new Product object with the details from the model
                var entity = new Product() {
                    Name = model.Name,
                    Url = model.Url,
                    Price = model.Price,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };

                // If the product is created successfully, display a success message
                if(_productService.Create(entity)) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Product Created.",
                        Message = $"{entity.Name} was created successfully.",
                        AlertType = "success"
                    });                    
                    return RedirectToAction("ProductList");
                }
                // If the product is not created successfully, display an error message
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
            
            // Create a new ProductModel object with the details from the entity
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

            // Get all the categories and add them to the ViewBag
            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds, IFormFile file) {
            if(ModelState.IsValid) {
                // Find the product by id
                var entity = _productService.GetById(model.ProductId);
                
                if (entity == null) {
                    return NotFound();
                }

                // Update the product details
                entity.Name = model.Name;
                entity.Url = model.Url;
                entity.Price = model.Price;
                entity.Description = model.Description;
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;

                // If a new image is uploaded, save the image to the wwwroot/img folder
                if(file != null) {
                    var extension = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);
                    
                    // Using statement to ensure the file stream is closed after the file is saved
                    using(var stream = new FileStream(path, FileMode.Create)) {
                        await file.CopyToAsync(stream); // Save the file to the path
                    }
                }

                // Update the product in the database
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
                // Create a new Category object with the details from the model
                var entity = new Category() {
                    Name = model.Name,
                    Url = model.Url,
                };

                // If the category is created successfully, display a success message
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

            // Find the category by id
            var entity = _categoryService.GetByIdWithProducts((int)id);
            
            if (entity == null) {
                return NotFound();
            }
            
            // Create a new CategoryModel object with the details from the entity
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
                // Find the category by id
                var entity = _categoryService.GetById(model.CategoryId);
                
                if (entity == null) {
                    return NotFound();
                }

                // Update the category details
                entity.Name = model.Name;
                entity.Url = model.Url;

                // If the category is updated successfully, display a success message
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
            // If the category is found, delete the category
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
    
        // Delete products from a category
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId) {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/" + categoryId);
        }

    }
}