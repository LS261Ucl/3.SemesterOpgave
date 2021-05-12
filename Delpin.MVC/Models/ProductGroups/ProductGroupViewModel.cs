using Delpin.MVC.Dto.v1.ProductCategories;
using Delpin.MVC.Dto.v1.Products;
using System;
using System.Collections.Generic;

namespace Delpin.Mvc.Models.ProductGroups
{
    public class ProductGroupViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }
        public ICollection<ProductDto> Products { get; set; }
    }
}
