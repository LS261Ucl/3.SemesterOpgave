using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.MVC.Dto.v1.RentalLines
{
    public class CreateRentalLineDto
    {
        [Required]
        public Guid ProductItemId { get; set; }
    }
}
