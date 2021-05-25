using Delpin.MVC.Dto.v1.PostalCities;
using Delpin.MVC.Dto.v1.RentalLines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Delpin.MVC.Dto.v1.Rentals
{
    public class RentalDto
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public PostalCityDto PostalCity { get; set; }
        public ICollection<RentalLineDto> RentalLines { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}