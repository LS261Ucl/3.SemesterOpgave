using Delpin.Application.Contracts.v1.RentalLines;
using System;
using System.Collections.Generic;

namespace Delpin.Application.Contracts.v1.Rentals
{
    public class UpdateRentalDto
    {
        public string CustomerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public ICollection<CreateRentalLineDto> RentalLines { get; set; }
    }
}