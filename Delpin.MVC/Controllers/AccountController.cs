using Delpin.MVC.Dto.v1.Identity;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Delpin.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpService _httpService;

        public AccountController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var response = await _httpService.Post<LoginDto, UserDto>("account/login", loginDto);

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
