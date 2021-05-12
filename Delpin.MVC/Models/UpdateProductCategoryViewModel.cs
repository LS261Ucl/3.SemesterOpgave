using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Mvc.Models
{
    public class UpdateProductCategoryViewModel
    {
        [Display(Name = "Navn")]
        public string Name { get; set; }
        [Display(Name = "Upload billede")]
        [FileExtensions(Extensions = "jpg,png,jpeg")]
        public IFormFile Image { get; set; }
    }
}
