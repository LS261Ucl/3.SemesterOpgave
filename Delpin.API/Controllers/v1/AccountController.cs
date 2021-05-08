﻿using System.Threading.Tasks;
using Delpin.Application.Contracts.v1.Identity;
using Delpin.Domain.Entities;
using Delpin.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
    }
}