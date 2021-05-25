using Delpin.Application.Contracts.v1.RentalLines;
using Delpin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Application.Contracts.v1.Rentals
{
    public class RentalDto
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public PostalCity PostalCity { get; set; }
        public ICollection<RentalLineDto> RentalLines { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}