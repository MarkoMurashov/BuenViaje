using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetStore.Models;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        #region fields

        RoleManager<IdentityRole> _roleManager;
        UserManager<ApplicationUser> _userManager;
        private IOrderRepository _repository;

        #endregion

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            IOrderRepository repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewBag.Current = "Roles";

            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UserList()
        {
            ViewBag.Current = "Users";

            var users = _repository.Orders.GroupBy(p => p.Email).Select(g => new
            {
                Name = g.Key,
                Count = g.Count(),
            });

            var userList = _userManager.Users;

            var vipList = new List<ApplicationUser>();
            foreach(var user in users)
            {
                if (user.Count >= 5)
                {
                    var tmp = userList.FirstOrDefault(o => o.Email == user.Name);
                    tmp.IsVIP = true;
                    vipList.Add(tmp);
                }
            }

            userList = userList.Union(vipList).Distinct();

            return View(userList.ToArray());
        }

        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleModel model = new ChangeRoleModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // получаем пользователя
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}