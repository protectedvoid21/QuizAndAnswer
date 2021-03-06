using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizAndAnswer.Models;
using QuizAndAnswer.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAndAnswer.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public ViewResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AppUser user) {
            if(!ModelState.IsValid) {
                return View();
            }

            AppUser userByEmail = await userManager.FindByEmailAsync(user.Email);

            if(userByEmail == null) {
                IdentityResult result = await userManager.CreateAsync(user, user.PasswordHash);

                if(result.Succeeded) {
                    ViewBag.Message = "New account created!";
                }
                else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }
            else {
                ViewBag.Message = "User with same email found";
                return View();
            }
            return View("LoggedIn");
        }

        [HttpGet]
        public ViewResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel) {
            if(!ModelState.IsValid) {
                return View();
            }

            AppUser userByEmail = await userManager.FindByEmailAsync(loginModel.Email);
            if(userByEmail == null) {
                ViewBag.NotValidMessage = "Incorrect email or password!";
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(userByEmail, loginModel.Password, false, false);

            if(!result.Succeeded) {
                ViewBag.NotValidMessage = "Incorrect email or password!";
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<ViewResult> Informations() {
            AppUser user = await userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpGet, Authorize]
        public ViewResult ChangePassword() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel passwordViewModel) {
            if(!ModelState.IsValid) {
                return View();
            }

            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            IdentityResult result = await userManager.ChangePasswordAsync(user, passwordViewModel.CurrentPassword, passwordViewModel.NewPassword);

            if(!result.Succeeded) {
                foreach(var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction("PasswordChanged");
        }

        [Authorize]
        public ViewResult PasswordChanged() {
            return View();
        }

        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
