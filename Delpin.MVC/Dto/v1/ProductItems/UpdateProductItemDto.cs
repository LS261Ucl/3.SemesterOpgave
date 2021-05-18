using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.MVC.Dto.v1.ProductItems
{
    public class UpdateProductItemDto
    {
        [Display(Name = "Er tilgængelig?")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Sidst serviceret")]
        public DateTime LastService { get; set; }

        [Display(Name = "Postnummer")]
        [Range(0, 9999)]
        public string PostalCode { get; set; }
    }
}
