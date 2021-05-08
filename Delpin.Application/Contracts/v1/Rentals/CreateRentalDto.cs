using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Delpin.Application.Contracts.v1.RentalLines;

namespace Delpin.Application.Contracts.v1.Rentals
{
    public class CreateRentalDto
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public ICollection<CreateRentalLineDto> RentalLines { get; set; }
    }
}