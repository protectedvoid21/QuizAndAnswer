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
    [Authorize(Roles = "Admin")]            
    public class AdminController : Controller {
        private readonly RoleManager<AppRole> roleManager;

        public AdminController(RoleManager<AppRole> roleManager) {
            this.roleManager = roleManager;
        }

        public IActionResult Panel() {
            return View();
        }

        public IActionResult RoleList() {
            IEnumerable<AppRole> roleList = roleManager.Roles;
            return View(roleList);
        }

        [HttpGet]
        public IActionResult CreateRole() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel roleViewModel) {
            bool roleExist = await roleManager.RoleExistsAsync(roleViewModel.Name);

            if(!roleExist) {
                await roleManager.CreateAsync(new AppRole { Name = roleViewModel.Name });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
