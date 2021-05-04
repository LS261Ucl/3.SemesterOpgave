using System;

namespace Delpin.Application.Contracts.v1.ProductCategories
{
    public class ProductCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        //public ICollection<ProductGroupDto> ProductGroups { get; set; }
    }
}