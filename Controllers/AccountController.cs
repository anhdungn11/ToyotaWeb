using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ToyotaWeb.Models;

namespace ToyotaWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;

            _signInManager = signInManager;

            _emailSender = emailSender;
        }

        // =====================================================
        // LOGIN
        // =====================================================

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ================= FIND USER =================

            var user =
                await _userManager.FindByEmailAsync(
                    model.Email.Trim()
                );

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Tài khoản không tồn tại"
                );

                return View(model);
            }

            // ================= RESET LOCK =================

            user.AccessFailedCount = 0;

            user.LockoutEnd = null;

            await _userManager.UpdateAsync(user);

            // ================= LOGIN =================

            var result =
                await _signInManager.PasswordSignInAsync(
                    user.UserName!,
                    model.Password,
                    false,
                    false
                );

            // ================= SUCCESS =================

            if (result.Succeeded)
            {
                // ================= ADMIN =================

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction(
                        "Index",
                        "Dashboard",
                        new { area = "Admin" }
                    );
                }

                // ================= SALE =================

                if (await _userManager.IsInRoleAsync(user, "Sale"))
                {
                    return RedirectToAction(
                        "Index",
                        "Dashboard", new { area = "Sale" }
                    );
                }

                // ================= ACCOUNTANT =================

                if (await _userManager.IsInRoleAsync(user, "Accountant"))
                {
                    return RedirectToAction(
                        "Index",
                        "Dashboard",
                        new { area = "Accountant" }
                    );
                }

                // ================= CUSTOMER =================

                if (await _userManager.IsInRoleAsync(user, "Customer"))
                {
                    return RedirectToAction(
                        "Profile",
                        "Customer"
                    );
                }

                // ================= DEFAULT =================

                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }

            // ================= FAILED =================

            ModelState.AddModelError(
                "",
                "Sai mật khẩu"
            );

            return View(model);
        }

        // =====================================================
        // REGISTER
        // =====================================================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ================= CHECK EXIST =================

            var existed =
                await _userManager.FindByEmailAsync(
                    model.Email.Trim()
                );

            if (existed != null)
            {
                ModelState.AddModelError(
                    "",
                    "Email đã tồn tại"
                );

                return View(model);
            }

            // ================= CREATE USER =================

            var user = new ApplicationUser
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.Phone,
                FullName = model.FullName,
                Address = model.Address,
                EmailConfirmed = true
            };

            var result =
                await _userManager.CreateAsync(
                    user,
                    model.Password
                );

            // ================= SUCCESS =================

            if (result.Succeeded)
            {
                // CUSTOMER ROLE

                await _userManager.AddToRoleAsync(
                    user,
                    "Customer"
                );

                TempData["success"] =
                    "Đăng ký thành công"; return RedirectToAction("Login");
            }

            // ================= ERROR =================

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description
                );
            }

            return View(model);
        }

        // =====================================================
        // LOGOUT
        // =====================================================

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(
                "Login",
                "Account"
            );
        }

        // =====================================================
        // FORGOT PASSWORD
        // =====================================================

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(
            string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(
                    "",
                    "Vui lòng nhập email"
                );

                return View();
            }

            var user =
                await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var token =
                    await _userManager
                    .GeneratePasswordResetTokenAsync(user);

                token =
                    System.Net.WebUtility.UrlEncode(token);

                var link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new
                    {
                        token,
                        email = user.Email
                    },
                    Request.Scheme
                );

                await _emailSender.SendEmailAsync(
                    user.Email!,
                    "Reset mật khẩu",
                    $"Click vào link để reset: <a href='{link}'>Reset Password</a>"
                );
            }

            TempData["success"] =
                "Nếu email tồn tại, link reset đã được gửi";

            return RedirectToAction("Login");
        }

        // =====================================================
        // RESET PASSWORD
        // =====================================================

        [HttpGet]
        public IActionResult ResetPassword(
            string token,
            string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Login");
            }

            return View(new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user =
                await _userManager.FindByEmailAsync(
                    model.Email
                );

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var decodedToken =
                System.Net.WebUtility.UrlDecode(
                    model.Token
                );

            var result =
                await _userManager.ResetPasswordAsync(
                    user,
                    decodedToken!,
                    model.Password
                );

            if (result.Succeeded)
            {
                // RESET LOCK

                user.AccessFailedCount = 0;

                user.LockoutEnd = null;

                await _userManager.UpdateAsync(user);

                TempData["success"] = "Đặt lại mật khẩu thành công";
                return RedirectToAction("Login");

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description
                );
            }
            return View(model);
        }
    }
}