using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandevuYonetimSistemi.Models;
using System.ComponentModel.DataAnnotations;

namespace RandevuYonetimSistemi.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        private string NormalizeReturnUrl(string? returnUrl, string defaultUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl)) return defaultUrl;
            // güvenlik: sadece local url'lere izin ver
            return Url.IsLocalUrl(returnUrl) ? returnUrl : defaultUrl;
        }

        private static bool IsAdminReturnUrl(string returnUrl)
            => returnUrl.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase);

        // GET: /Auth/Register?returnUrl=/Member
        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            var safeReturnUrl = NormalizeReturnUrl(returnUrl, "/Member");

            // ✅ Admin kayıt kapalı
            if (IsAdminReturnUrl(safeReturnUrl))
                return RedirectToAction("Login", new { returnUrl = "/Admin" });

            ViewBag.ReturnUrl = safeReturnUrl;
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm vm, string? returnUrl = null)
        {
            var safeReturnUrl = NormalizeReturnUrl(returnUrl, "/Member");

            // ✅ Admin kayıt kapalı
            if (IsAdminReturnUrl(safeReturnUrl))
                return RedirectToAction("Login", new { returnUrl = "/Admin" });

            ViewBag.ReturnUrl = safeReturnUrl;

            if (!ModelState.IsValid) return View(vm);

            var email = vm.Email.Trim().ToLowerInvariant();

            // aynı email var mı?
            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
            {
                ModelState.AddModelError("", "Bu email ile zaten kayıt var. Giriş yapabilirsin.");
                return View(vm);
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError("", e.Description);
                return View(vm);
            }

            // ✅ Member rolü yoksa oluştur
            if (!await _roleManager.RoleExistsAsync("Member"))
                await _roleManager.CreateAsync(new IdentityRole("Member"));

            // ✅ Kayıt olan kullanıcıya Member rolü ver
            await _userManager.AddToRoleAsync(user, "Member");

            // ✅ login
            await _signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(safeReturnUrl);
        }

        // GET: /Auth/Login?returnUrl=/Member
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            var safeReturnUrl = NormalizeReturnUrl(returnUrl, "/Member");
            ViewBag.ReturnUrl = safeReturnUrl;
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm vm, string? returnUrl = null)
        {
            var safeReturnUrl = NormalizeReturnUrl(returnUrl, "/Member");
            ViewBag.ReturnUrl = safeReturnUrl;

            if (!ModelState.IsValid) return View(vm);

            var email = vm.Email.Trim().ToLowerInvariant();

            var result = await _signInManager.PasswordSignInAsync(
                userName: email,
                password: vm.Password,
                isPersistent: vm.RememberMe,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Giriş başarısız. Email/Şifre yanlış olabilir.");
                return View(vm);
            }

            return LocalRedirect(safeReturnUrl);
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Portal", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

    public class RegisterVm
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Şifreler aynı değil.")]
        public string ConfirmPassword { get; set; } = "";
    }

    public class LoginVm
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }
    }
}
