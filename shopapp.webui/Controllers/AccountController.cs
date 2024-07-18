using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController: Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string ReturnUrl=null) {
            
            return View(new LoginModel()
                {
                    ReturnUrl = ReturnUrl
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model) {
            if(!ModelState.IsValid) {
                return View(model);
            }
            // var user = await _userManager.FindByNameAsync(model.UserName);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null) {
                ModelState.AddModelError("", "User not found");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user)) {
                ModelState.AddModelError("", "Please confirm your email");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if(result.Succeeded) {
                return Redirect(model.ReturnUrl??"~/");
            }

            ModelState.AddModelError("", "Username or password is wrong");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model) {
            if (!ModelState.IsValid) {
                return View();
            }
            var user = new User() {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded) {
                // generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new {
                    userId = user.Id,
                    token = code
                });
                System.Console.WriteLine(url);

                // email

                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("Password", "Password should contain at least 1 uppercase, 1 lowercase, 1 number and 1 special character");
            return View(model);
        }
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token) {
            if (userId == null || token == null) {
                CreateMessage("Invalid token.", "danger");
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user != null) {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded) {
                    CreateMessage("Your email has been confirmed.", "success");
                    return View();
                }
            }
            CreateMessage("Your email could not be confirmed.", "warning");
            return View();
        }

        private void CreateMessage(string message, string alertType) {
            var msg = new AlertMessage() {
                Message = message,
                AlertType = alertType
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }

}