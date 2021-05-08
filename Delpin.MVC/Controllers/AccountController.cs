using Delpin.MVC.Dto.v1.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Delpin.MVC.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            return Ok();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            return Ok();
        }
    }
}
