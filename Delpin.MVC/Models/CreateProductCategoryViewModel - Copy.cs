using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Mvc.Models
{
    public class CreateProductCategoryViewModel
    {
        [Required]
        [Display(Name = "Navn")]
        public string Name { get; set; }
        [Display(Name = "Upload billede")]
        [FileExtensions(Extensions = "jpg,png,jpeg")]
        public IFormFile Image { get; set; }
    }
}
