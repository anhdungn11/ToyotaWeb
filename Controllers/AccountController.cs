using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Models;
using System.Threading.Tasks;

namespace ToyotaWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            false,
            false);

        if (result.Succeeded)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

            if (await _signInManager.UserManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Sai email hoặc mật khẩu");
    }

    return View(model);
}

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}