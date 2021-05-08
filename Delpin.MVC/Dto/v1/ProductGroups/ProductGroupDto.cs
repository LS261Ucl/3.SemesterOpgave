using Delpin.MVC.Dto.v1.ProductCategories;
using Delpin.MVC.Dto.v1.Products;
using System;
using System.Collections.Generic;

namespace Delpin.MVC.Dto.v1.ProductGroups
{
    public class ProductGroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }
        public ICollection<ProductDto> Products { get; set; }
    }
}