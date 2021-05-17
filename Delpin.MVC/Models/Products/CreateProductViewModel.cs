using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Mvc.Models.Products
{
    public class CreateProductViewModel
    {
        [Required]
        [Display(Name = "Navn")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Pris")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Upload billede")]
        [FileExtensions(Extensions = "jpg,png,jpeg")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        [Required]
        [Display(Name = "Produktgruppe")]
        public Guid ProductGroupId { get; set; }
    }
}