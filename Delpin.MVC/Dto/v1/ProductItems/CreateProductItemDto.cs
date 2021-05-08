using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.MVC.Dto.v1.ProductItems
{
    public class CreateProductItemDto
    {
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public DateTime LastService { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string PostalCode { get; set; }
    }
}
