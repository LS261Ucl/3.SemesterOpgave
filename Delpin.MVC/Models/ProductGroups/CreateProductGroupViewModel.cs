using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Mvc.Models.ProductGroups
{
    public class CreateProductGroupViewModel
    {
        [Required]
        [Display(Name = "Navn")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Upload billede")]
        [FileExtensions(Extensions = "jpg,png,jpeg")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        [Required]
        [Display(Name = "Produkt Kategori")]
        public Guid ProductCategoryId { get; set; }
    }
}