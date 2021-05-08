using System;
using System.ComponentModel.DataAnnotations;

namespace Delpin.MVC.Dto.v1.ProductGroups
{
    public class CreateProductGroupDto
    {
        [Required]
        public string Name { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public Guid ProductCategoryId { get; set; }
    }
}
