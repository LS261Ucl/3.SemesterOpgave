using Delpin.MVC.Dto.v1.Identity;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Delpin.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpService _httpService;
        private readonly HttpClient _httpClient;

        public AccountController(IHttpService httpService, HttpClient httpClient)
        {
            _httpService = httpService;
            _httpClient = httpClient;
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

            if (!response.Success)
                return View(loginDto);

            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("Name");
            Response.Cookies.Delete("Role");

            Response.Cookies.Append("Token", response.Response.Token, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(5),
                Secure = true,
                HttpOnly = true
            });

            Response.Cookies.Append("Name", response.Response.FullName, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(5),
                Secure = true,
                HttpOnly = true
            });

            var jwt = new JwtSecurityToken(response.Response.Token);

            Response.Cookies.Append("Role", jwt.Claims.FirstOrDefault(x => x.Type == "role")!.Value, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(5),
                Secure = true,
                HttpOnly = true
            });

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Response.Token);

            return RedirectToAction("Index", "Home");
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

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Append("Token", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddSeconds(-1),
            });

            Response.Cookies.Append("Name", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddSeconds(-1),
            });

            Response.Cookies.Append("Role", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddSeconds(-1),
            });

            _httpClient.DefaultRequestHeaders.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
