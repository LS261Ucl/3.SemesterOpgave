using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Mvc.Models.ProductGroups
{
    public class UpdateProductGroupViewModel
    {
        [Display(Name = "Navn")]
        public string Name { get; set; }

        [Display(Name = "Upload billede")]
        [FileExtensions(Extensions = "jpg,png,jpeg")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        [Display(Name = "Produkt Kategori")]
        public Guid ProductCategoryId { get; set; }

        public byte[] ImageTemp { get; set; }
    }
}
