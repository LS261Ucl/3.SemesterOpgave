using Delpin.MVC.Dto.v1.Products;
using Delpin.MVC.Dto.v1.RentalLines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Mvc.Models.Rentals
{
    public class CreateRentalViewModel
    {
        [Required]
        [Display(Name = "Kundenummer")]
        public string CustomerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start dato")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Slut dato")]
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(5);

        [Required]
        [Display(Name = "Adresse")]
        public string Address { get; set; }

        [Required]
        [Range(0, 9999)]
        [Display(Name = "Postnummer")]
        public string PostalCode { get; set; }

        [Required] public List<CreateRentalLineDto> RentalLines { get; set; } = new List<CreateRentalLineDto>();

        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}