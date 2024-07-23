using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shopapp.business.Abstract;
using shopapp.webui.EmailServices;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController: Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService) {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;
        }

        public IActionResult Login(string ReturnUrl=null) {
            return View(new LoginModel() {
                    ReturnUrl = ReturnUrl
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model) {
            if(!ModelState.IsValid) {
                return View(model);
            }
            
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null) {
                ViewBag.LoginFail = "This email is not registered.";
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user)) {
                ViewBag.LoginFail = "Please confirm your email.";
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if(result.Succeeded) {
                return Redirect(model.ReturnUrl??"~/");
            }
            ViewBag.LoginFail = "Password is incorrect.";
                
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

            if (_userManager.FindByEmailAsync(model.Email).Result != null) {
                TempData.Put("message", new AlertMessage() {
                    Title = "Email Error.",
                    Message = "This email is already registered.",
                    AlertType = "danger"
                });
                return View();
            }

            var user = new User() {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                City = model.City,
                Country = model.Country,
                ZipCode = model.ZipCode
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded) {
                // generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new {
                    userId = user.Id,
                    token = code
                });

                // email
                // await _emailSender.SendEmailAsync(model.Email, "Confirm your email", $"Click <a href='https://localhost:5001{url}'>here</a> to confirm your email.");
                TempData.Put("message", new AlertMessage() {
                    Title = "Confirm Email.",
                    Message = "A confirmation link has been sent to your email. Please go to your email and confirm your email.",
                    AlertType = "warning"
                });

                return RedirectToAction("Login", "Account");
            }
            var PasswordError = @"
                                    <p>Password must be at least 6 characters long and must contain at least</p>
                                    <ul>
                                        <li>one uppercase letter</li>
                                        <li>one lowercase letter</li>
                                        <li>one digit</li>
                                        <li>one special character</li>
                                    </ul>
                                ";
            TempData.Put("message", new AlertMessage() {
                Title = "Password Error",
                Message = PasswordError,
                AlertType = "danger"
            });
            return View(model);
        }
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            TempData.Put("message", new AlertMessage() {
                Title = "Logged Out.",
                Message = "You have logged out successfully.",
                AlertType = "warning"
            });
            return Redirect("~/");
        }

        public IActionResult ForgotPassword() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email) {
            if(string.IsNullOrEmpty(Email)) {
                ViewBag.Status = "Email is required.";
                return View();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            
            if(user == null) {
                ViewBag.Status = "Email not found.";
                return View();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            // generate token
            var url = Url.Action("ResetPassword", "Account", new {
                userId = user.Id,
                token = code
            });

            // email
            await _emailSender.SendEmailAsync(Email, "Reset Password", $"Click <a href='https://localhost:5001{url}'>here</a> to reset your password.");
            
            TempData.Put("message", new AlertMessage() {
                Title = "Reset Password.",
                Message = "A password reset link has been sent to your email.",
                AlertType = "warning"
            });

            return View();
        }

        public IActionResult ResetPassword(string userId, string token) {
            if(userId == null || token == null) {
                return RedirectToAction("Home", "Index");
            }
            var model = new ResetPasswordModel() {
                Token = token
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null) {
                return RedirectToAction("Index", "Home");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded) {
                TempData.Put("message", new AlertMessage() {
                    Title = "Password Reset.",
                    Message = "Your password was reset successfully.",
                    AlertType = "success"
                });
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token) {
            if (userId == null || token == null) {
                TempData.Put("message", new AlertMessage() {
                    Title = "Invalid token.",
                    Message = "Invalid token.",
                    AlertType = "danger"
                });
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user != null) {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded) {
                    _cartService.InitializeCart(user.Id);
                    TempData.Put("message", new AlertMessage() {
                        Title = "Email Confirmed.",
                        Message = "Your email has been confirmed.",
                        AlertType = "success"
                    });
                    return View();
                }
            }
            TempData.Put("message", new AlertMessage() {
                Title = "Email not confirmed.",
                Message = "Your email could not be confirmed.",
                AlertType = "warning"
            });
            return View();
        }

        public IActionResult AccessDenied() {
            return View();
        }
    }

}