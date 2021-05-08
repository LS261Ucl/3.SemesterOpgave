using System.ComponentModel.DataAnnotations;

namespace Delpin.MVC.Dto.v1.ProductCategories
{
    public class CreateProductCategoryDto
    {
        [Required]
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}