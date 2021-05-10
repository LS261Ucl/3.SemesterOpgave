using Delpin.MVC.Dto.v1.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required]
        public RegisterDto RegisterDto { get; set; } = new RegisterDto();

        [Required]
        public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Passwords don't match")]
        public bool EqualPasswords => RegisterDto.Password == ConfirmPassword;
    }
}
