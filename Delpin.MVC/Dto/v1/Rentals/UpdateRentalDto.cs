using Delpin.MVC.Dto.v1.RentalLines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Delpin.MVC.Dto.v1.Rentals
{
    public class UpdateRentalDto
    {
        [Display(Name = "Kundenummer")]
        public string CustomerId { get; set; }

        [Display(Name = "Start dato")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Slut dato")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Adresse")]
        public string Address { get; set; }

        [Display(Name = "Postnummer")]
        [Range(0, 9999)]
        public string PostalCode { get; set; }

        public List<CreateRentalLineDto> RentalLines { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}