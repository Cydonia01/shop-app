/*
* AccountController.cs handles the user account operations.
* This file includes the methods for login, register, logout, forgot password, reset password, confirm email, profile, change password.
*/
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.webui.EmailServices;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [AutoValidateAntiforgeryToken] // Prevents Cross-Site Request Forgery (CSRF) attacks
    public class AccountController: Controller
    {
        // Dependency Injection
        // Defines the managers and services that will be used in the controller
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ICartService _cartService;

        // Constructor: Initializes the AccountController class with the UserManager, SignInManager, EmailSender, and CartService parameters.
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService) {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = null) {
            return View(new LoginModel() {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model) {
            if(!ModelState.IsValid) {
                return View(model);
            }
            
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null) {
                ViewBag.LoginFail = "This email is not registered.";
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user)) {
                ViewBag.LoginFail = "Please confirm your email.";
                return View(model);
            }

            // Sign in the user
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

            // Check if the email is already registered
            if (_userManager.FindByEmailAsync(model.Email).Result != null) {
                TempData.Put("message", new AlertMessage() {
                    Title = "Email Error.",
                    Message = "This email is already registered.",
                    AlertType = "danger"
                });
                return View();
            }
            // Check if the username is already registered
            if (_userManager.FindByNameAsync(model.UserName).Result != null) {
                TempData.Put("message", new AlertMessage() {
                    Title = "Username Error.",
                    Message = "This username is already registered.",
                    AlertType = "danger"
                });
                return View();
            }

            // Create a new user
            var user = new User() {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                City = model.City,
                Country = model.Country,
                ZipCode = model.ZipCode
            };

            // Register the user
            var result = await _userManager.CreateAsync(user, model.Password);

            // If the user is registered successfully, generate a token and send an email to the user
            if(result.Succeeded) {
                // Generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new {
                    userId = user.Id,
                    token = code
                });

                // Send email
                await _emailSender.SendEmailAsync(model.Email, "Confirm your email", $"Click <a href='https://localhost:5001{url}'>here</a> to confirm your email.");

                // Send a message and redirect the user to the login page
                TempData.Put("message", new AlertMessage() {
                    Title = "Confirm Email.",
                    Message = "A confirmation link has been sent to your email. Please go to your email and confirm your email.",
                    AlertType = "warning"
                });

                return RedirectToAction("Login", "Account");
            }
            else if(result.Errors.Count() > 0) {
                PasswordError();
            }
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
            
            // Generate token
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new {
                userId = user.Id,
                token = code
            });
            // Send email
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
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if(user == null) {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            
            // If the password is reset successfully, send a message and redirect the user to the login page
            if (result.Succeeded) {
                TempData.Put("message", new AlertMessage() {
                    Title = "Password Reset.",
                    Message = "Your password was reset successfully.",
                    AlertType = "success"
                });
                return RedirectToAction("Login", "Account");
            }
            // If the password is not reset successfully, send an error message
            else if (result.Errors.Count() > 0) {
                PasswordError();
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
                // If the email is confirmed successfully, send a message and initialize the cart
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

        // Profile page
        public async Task<IActionResult> Profile() {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (user == null) {
                return NotFound();
            }
            // Create the ProfileModel object
            var model = new ProfileModel() {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Country = user.Country,
            };
            ViewBag.SelectedPage = "Profile";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileModel model) {
            ViewBag.SelectedPage = "Profile";
            if (!ModelState.IsValid) {
                return View();
            }
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (user != null) {
                // Update the user's information
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.City = model.City;
                user.Country = model.Country;
                var result = await _userManager.UpdateAsync(user);

                // If the user's information is updated successfully, send a message and redirect the user to the profile page
                if (result.Succeeded) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Profile Updated.",
                        Message = "Your profile has been updated successfully.",
                        AlertType = "success"
                    });
                    return RedirectToAction("Profile");
                }
                else {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Profile Error.",
                        Message = "Your profile could not be updated.",
                        AlertType = "danger"
                    });
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword() {
            ViewBag.SelectedPage = "ChangePassword";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordModel model) {
            ViewBag.SelectedPage = "ChangePassword";
            if (!ModelState.IsValid) {
                return View();
            }

            // Find the user
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));

            // If the user is found, change the password
            if (user != null) {
                // If the current password is incorrect, send an error message
                if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword)) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Password Error.",
                        Message = "Your current password is incorrect.",
                        AlertType = "danger"
                    });
                    return View();
                }

                // If the new password is the same as the current password, send an error message
                if (await _userManager.CheckPasswordAsync(user, model.NewPassword)) {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Password Error.",
                        Message = "New password cannot be the same as the current password.",
                        AlertType = "danger"
                    });
                    return View();
                }
                
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                // If the password is changed successfully, send a message and redirect the user to the login page
                if (result.Succeeded) {
                    await _signInManager.SignOutAsync();
                    TempData.Put("message", new AlertMessage() {
                        Title = "Password Changed.",
                        Message = "Your password has been changed successfully. Please login with your new password.",
                        AlertType = "success"
                    });
                    return RedirectToAction("Login");
                }
                
                // If the password is not changed successfully, send an error message
                if (result.Errors.Count() > 0) {
                    PasswordError();
                }
            }
            return View(model);
        }
    
        // Password error message in case the password does not meet the requirements in html format
        public void PasswordError() {
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
        }
    }
}