using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetStore.Models.ViewModels;
using PetStore.Models;

namespace PetStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userMgr,
                SignInManager<ApplicationUser> signInMgr)
        {
            _userManager = userMgr;
            _signInManager = signInMgr;
             // IdentitySeedData.EnsurePopulated(userMgr).Wait();
        }

        [AllowAnonymous]
        public ViewResult Register(string returnUrl)
        {
            return View(new RegisterModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email, Birthday = model.Birthday };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user =
                    await _userManager.FindByNameAsync(loginModel.Name);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    if ((await _signInManager.PasswordSignInAsync(user,
                            loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/");
                    }
                }
            }
            ModelState.AddModelError("", "Некорректное имя пользователя или пароль");
            return View(loginModel);
        }

        public async Task<RedirectToActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View(new EditModel());
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed()
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Logout", "Account");
                }
            }

            return Redirect("/");
        }

        public async Task<ActionResult> Edit()
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                EditModel model = new EditModel { Birthday = user.Birthday };

                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditModel model)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                user.Birthday = model.Birthday;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Redirect(model?.ReturnUrl ?? "/");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return View(model);
        }
    }
}