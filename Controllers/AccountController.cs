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
            var result = await signInManager.PasswordSignInAsync(userByEmail, loginModel.Password, false, false);

            if(result.Succeeded) {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.NotValidMessage = "Incorrect email or password!";
            return View();
        }

        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
