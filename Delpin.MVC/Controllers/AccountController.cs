using Delpin.Mvc.Models;
using Delpin.MVC.Dto.v1.Identity;
using Delpin.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var response = await _httpService.Create<LoginDto, UserDto>("account/login", loginDto, null);

            if (!response.Success)
                return View(loginDto);

            var jwt = new JwtSecurityToken(response.Response.Token);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, response.Response.Email),
                new Claim(ClaimTypes.GivenName, response.Response.FullName),
                new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(x => x.Type == "role")!.Value),
                new Claim("Token", response.Response.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

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
        public async Task<IActionResult> Register(RegisterViewModel registerVm)
        {
            if (!ModelState.IsValid)
                return View(registerVm);

            var response = await _httpService.Create<RegisterDto, UserDto>("account/register", registerVm.RegisterDto, User.Claims.First(x => x.Type == "Token").Value);

            if (!response.Success)
                return View(registerVm);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _httpClient.DefaultRequestHeaders.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
