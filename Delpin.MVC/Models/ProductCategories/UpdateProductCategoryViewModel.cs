using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Delpin.Mvc.Models.ProductCategories
{
    public class UpdateProductCategoryViewModel
    {
        [Display(Name = "Navn")]
        public string Name { get; set; }
        [Display(Name = "Upload billede")]
        [FileExtensions(Extensions = "jpg,png,jpeg")]
        public IFormFile Image { get; set; }

        public byte[] ImageTemp { get; set; }
    }
}
