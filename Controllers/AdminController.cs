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
        private readonly UserManager<AppUser> userManager;

        public AdminController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager) {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Panel() {
            return View();
        }

        public IActionResult RoleList() {
            IEnumerable<AppRole> roleList = roleManager.Roles;
            return View(roleList);
        }

        public ViewResult UserList() {
            return View(userManager.Users);
        }

        [HttpGet]
        public async Task<ViewResult> EditUser(int id) {
            AppUser user = await userManager.FindByIdAsync(id.ToString());

            EditUserViewModel userViewModel = new EditUserViewModel {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            };
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel user) {
            if(!ModelState.IsValid) {
                return View();
            }

            AppUser updateUser = new AppUser();
            foreach(var editUser in userManager.Users) {
                if(editUser.Id == user.Id) {
                    editUser.UserName = user.UserName;
                    editUser.Email = user.Email;
                    updateUser = editUser;
                    break;
                }
            }
            IdentityResult result = await userManager.UpdateAsync(updateUser);

            if(!result.Succeeded) {
                foreach(var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction();
            }

            return RedirectToAction("UserList");
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRole(string id) {
            ViewBag.userId = id;

            var user = await userManager.FindByIdAsync(id);

            if(user == null) {
                ViewBag.Message = $"User with id : {id} not found";
                return View();
            }

            var model = new List<EditUserRoleViewModel>();
            AppRole[] roleList = roleManager.Roles.ToArray();
            foreach(var role in roleList) {
                var userRoleViewModel = new EditUserRoleViewModel {
                    RoleId = role.Id.ToString(),
                    RoleName = role.Name,
                };

                userRoleViewModel.IsSelected = await userManager.IsInRoleAsync(user, role.Name);
                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRole(List<EditUserRoleViewModel> editUserRoles, string id) {
            AppUser user = await userManager.FindByIdAsync(id);

            if(user == null) {
                ViewBag.Message = $"User with id : {id} not found";
                return View();
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if(!result.Succeeded) {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View();
            }

            result = await userManager.AddToRolesAsync(user, editUserRoles.Where(r => r.IsSelected == true).Select(y => y.RoleName));
        
            if(!result.Succeeded) {
                ModelState.AddModelError("", "Cannot add role to user");
                return View();
            }

            return RedirectToAction("UserList");
        }

        public async Task<ViewResult> DeleteUser(int id) {
            await userManager.DeleteAsync(await userManager.FindByIdAsync(id.ToString()));
            return View();
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
