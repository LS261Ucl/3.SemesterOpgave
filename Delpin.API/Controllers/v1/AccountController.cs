using Delpin.Application.Contracts.v1.Identity;
using Delpin.Domain.Entities.Identity;
using Delpin.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Delpin.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Login operation to access the applications services
        /// </summary>
        /// <param name="loginDto"></param>
        /// <remarks>
        /// Sample value of login
        ///
        ///     POST /Account/Login
        ///     {
        ///         "email" : "user@delpin.dk",
        ///         "password" : "password"
        ///     }
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                _logger.LogInformation("Bad email / password combination.");
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Bad email / password combination.");
                return Unauthorized();
            }

            return Ok(new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = _tokenService.CreateToken(user)
            });
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Email))
            {
                _logger.LogInformation($"User with email: {registerDto.Email} already exists");
                return BadRequest("User already exists");
            }

            var user = new AppUser
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Problem registering user");
                return BadRequest("Problem registering user");
            }

            return Ok(new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = _tokenService.CreateToken(user)
            });
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return Ok(new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = _tokenService.CreateToken(user)
            });
        }
    }
}