using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.Application.Contracts.v1.Products
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid ProductGroupId { get; set; }

        public byte[] Image { get; set; }
    }
}
