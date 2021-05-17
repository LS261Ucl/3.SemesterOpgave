using Delpin.MVC.Dto.v1.ProductCategories;
using System;

namespace Delpin.Mvc.Models.ProductGroups
{
    public class ProductGroupViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }
    }
}
