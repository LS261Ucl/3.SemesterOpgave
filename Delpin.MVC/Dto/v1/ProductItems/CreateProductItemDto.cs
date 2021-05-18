using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.MVC.Dto.v1.ProductItems
{
    public class CreateProductItemDto
    {
        [Required]
        [Display(Name = "Er tilgængelig?")]
        public bool IsAvailable { get; set; }

        [Required]
        [Display(Name = "Sidst serviceret")]
        public DateTime LastService { get; set; }

        [Required]
        [Display(Name = "Produkt")]
        public Guid ProductId { get; set; }

        [Required]
        [Display(Name = "Postnummer")]
        [Range(0, 9999)]
        public string PostalCode { get; set; }
    }
}
