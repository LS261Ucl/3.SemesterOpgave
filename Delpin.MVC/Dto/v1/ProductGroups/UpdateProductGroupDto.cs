using System;

namespace Delpin.MVC.Dto.v1.ProductGroups
{
    public class UpdateProductGroupDto
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
