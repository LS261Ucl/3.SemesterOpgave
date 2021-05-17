using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Mvc.Models.Products
{
    public class UpdateProductViewModel
    {
        [Display(Name = "Navn")]
        public string Name { get; set; }

        [Display(Name = "Pris")]
        public decimal Price { get; set; }

        [Display(Name = "Beskrivelse")]
        public string Description { get; set; }

        [Display(Name = "Upload billede")]
        [FileExtensions(Extensions = "jpg,png,jpeg")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        [Display(Name = "Produktgruppe")]
        public Guid ProductGroupId { get; set; }

        public byte[] ImageTemp { get; set; }
    }
}