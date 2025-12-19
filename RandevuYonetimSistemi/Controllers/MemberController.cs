using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RandevuYonetimSistemi.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : Controller
    {
        public IActionResult Index() => View();
    }

}
