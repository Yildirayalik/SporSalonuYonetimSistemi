using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandevuYonetimSistemi.Models;

namespace RandevuYonetimSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Admin panel ana sayfa
        public IActionResult Index()
        {
            return View();
        }

        // ⚠️ DEBUG / TEK SEFERLİK
        // /Admin/GrantMember?email=member@gmail.com
        [HttpGet]
        public async Task<IActionResult> GrantMember(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email gerekli");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı");

            if (!await _userManager.IsInRoleAsync(user, "Member"))
            {
                await _userManager.AddToRoleAsync(user, "Member");
                return Ok("Member rolü verildi ✅");
            }

            return Ok("Kullanıcı zaten Member rolünde");
        }
    }
}
