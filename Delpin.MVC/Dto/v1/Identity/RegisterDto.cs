using System.ComponentModel.DataAnnotations;

namespace Delpin.MVC.Dto.v1.Identity
{
    public class RegisterDto
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&+=£$€*-]).{6,18}$",
            ErrorMessage =
                "Password must contain at least one lower case letter, one upper case letter, one digit and one special character")]
        public string Password { get; set; }
    }
}