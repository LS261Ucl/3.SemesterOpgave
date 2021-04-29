using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delpin.Domain.Entities
{
    public class Rental : BaseEntity
    {
        public string CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }
        [ForeignKey(nameof(PostalCode))]
        public PostalCity PostalCity { get; set; }

        public ICollection<RentalLine> RentalLines { get; set; }
    }
}