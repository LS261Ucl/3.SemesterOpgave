using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Mvc.Models.ProductCategories
{
    public class CreateProductCategoryViewModel
    {
        [Required]
        [Display(Name = "Navn")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vælg et billede.")]
        [Display(Name = "Upload billede")]
        [FileExtensions(Extensions = "jpg,png,jpeg")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
